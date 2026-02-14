using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContarcts;
using ServiceContracts;
using ServiceContracts.DTO;


namespace Services
{    
    public class CountriesUploaderService : ICountriesUploaderService
    {
        //private field
        private readonly ICountriesRepository _countriesRepository;

        //constructor
        public CountriesUploaderService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;           
        }       

        public async Task<int> UploadCountriesFromExcelFile(IFormFile fromFile)
        {
            MemoryStream memroyStream = new MemoryStream();
            await fromFile.CopyToAsync(memroyStream);

            int countriesInserted = 0;

            ExcelPackage.License.SetNonCommercialPersonal("zebon");
            using(ExcelPackage exclePackage = new ExcelPackage(memroyStream))
            {
                ExcelWorksheet worksheet = exclePackage.Workbook.Worksheets["Countries"];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if(_countriesRepository.GetCountryByCountryName(countryName) == null)
                        {
                            Country country = new Country() { CountryName = countryName };                            
                            await _countriesRepository.AddCountry(country);

                            countriesInserted++;
                        }
                    }
                }
            }

            return countriesInserted;
        }
    }
}
