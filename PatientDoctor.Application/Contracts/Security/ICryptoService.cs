using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Security
{
    public interface ICryptoService
    {
        string CreateKey(string salt, string data);
        string CreateKey(string salt, string data, int keySizeInKb);
        bool CheckKey(string hash, string salt, string data);
        string CreateFingerprint(string data);
        string CreateSalt();
        string CreateSalt(int sizeInKb);
    }
}
