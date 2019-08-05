using PeopleViewer.Common;
using PeopleViewer.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PersonReader.Decorators
{
    public class ExceptionLoggingReader : IPersonReader
    {
        IPersonReader _wrappedReader;
        ILogger _logger;

        public ExceptionLoggingReader(IPersonReader wrappedReader, 
            ILogger logger)
        {
            _wrappedReader = wrappedReader;
            _logger = logger;
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            try
            {
                return await _wrappedReader.GetPeopleAsync();
            }
            catch (Exception ex)
            {
                await _logger?.LogException(ex);
                throw;
            }
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            try
            {
                return await _wrappedReader.GetPersonAsync(id);
            }
            catch (Exception ex)
            {
                await _logger?.LogException(ex);
                throw;
            }
        }
    }
}
