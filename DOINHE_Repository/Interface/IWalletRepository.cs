using DOINHE_BusinessObject;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public interface IWalletRepository
    {
        List<Wallet> GetAllWallets();
        Wallet GetWalletById(int id);
        void SaveWallet(Wallet wallet);
        void UpdateWallet(Wallet wallet);
        void DeleteWallet(Wallet wallet);
    }
}
