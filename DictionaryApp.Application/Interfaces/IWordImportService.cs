using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryApp.Application.Interfaces
{
    public interface IWordImportService
    {
        Task ImportWordsFromJson(string filePath);
    }
}
