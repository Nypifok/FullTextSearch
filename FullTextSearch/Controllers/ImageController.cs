using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullTextSearch.Datasets.Loader;
using FullTextSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullTextSearch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DataSetLoader dsLoader;
        private readonly ImageRepository db;

        public ImageController(DataSetLoader dsLoader,ImageRepository db)
        {
            this.db = db;
            this.dsLoader = dsLoader;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> LoadDataSet()
        {
            try
            {
                await dsLoader.LoadDataSetToDbAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
        [HttpGet("[action]/{query}")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                var result = await db.FullTextSearch(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
