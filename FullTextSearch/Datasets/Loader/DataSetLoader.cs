using CsvHelper;
using FullTextSearch.Models;
using FullTextSearch.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FullTextSearch.Datasets.Loader
{
    public class DataSetLoader
    {
        private string dataPath = @"D:\lenta-ru-news.csv";
        private readonly ImageRepository context;
        public DataSetLoader(ImageRepository context)
        {
            this.context = context;
        }
        public async Task LoadDataSetToDbAsync()
        {

            var packageSize = 200000;
            var taskList = new List<Task>();
            using (StreamReader streamReader = new StreamReader(dataPath))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var images = new List<Image>(packageSize);
                    csvReader.Configuration.Delimiter = ",";
                    csvReader.Configuration.BadDataFound = null;

                    int i=0;
                    while (csvReader.Read())
                    {
                        if (i%packageSize==0)
                        {
                            taskList.Add(context.CopyTo(images));
                            images = new List<Image>(packageSize);
                        }
                        var record = csvReader.GetRecord<Image>();
                        if (record != null)
                        {
                            record.Id = Guid.NewGuid();
                            if (record.Description.Length > 2000)
                                record.Description = record.Description.Substring(0, 2000);
                            images.Add(record);
                        }
                        i++;
                    }
                    taskList.Add(context.CopyTo(images));
                    await Task.WhenAll(taskList);
                   
                }
            }


        }
    }

}
