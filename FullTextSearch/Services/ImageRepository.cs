using FullTextSearch.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PostgreSQLCopyHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FullTextSearch.Services
{
    public class ImageRepository
    {
        private readonly FTSDBContext context;
        public ImageRepository(FTSDBContext context)
        {
            this.context = context;
        }
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Image>> FullTextSearch(string query,string weights,int limit)
        {
            
            var images= await context.Images
                .FromSqlInterpolated($@"SELECT * FROM images_fts({query},{weights},{limit});").ToListAsync();
            return images;
        
        }
        public async Task<IEnumerable<Image>> FullTextSearch(string query)
        {

            var images = await context.Images
                .FromSqlInterpolated($@"SELECT * FROM images_fts({query},'ABC',100);").ToListAsync();
            return images;

        }
        public async Task InsertAsync(IEnumerable<Image> images)
        {
            await context.Images.AddRangeAsync(images);
        }
        public async Task InsertAsync(Image images)
        {
            await context.Images.AddAsync(images);
        }

        public async Task<ulong> CopyTo(IEnumerable<Image> images)
        {
            var copyHelper = new PostgreSQLCopyHelper<Image>("images")
                .MapUUID("images_id", t => t.Id)
                .MapText("title", t => t.Title)
                .MapText("key_words", t => t.KeyWords)
                .MapText("description", t => t.Description);
            using (var connection = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Database=fts_db;Username=postgres;Password=30056010;"))
            {
                await connection.OpenAsync();
                return await copyHelper.SaveAllAsync(connection, images, new CancellationToken());
            }

        }
    }
}
