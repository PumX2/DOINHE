using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class WalletRepository : IWalletRepository
    {
        public List<Wallet> GetAllWallets() => WalletDAO.GetWallets();
        public Wallet GetWalletById(int id) => WalletDAO.GetWalletById(id);
        public void SaveWallet(Wallet wallet) => WalletDAO.InsertWallet(wallet);
        public void UpdateWallet(Wallet wallet) => WalletDAO.UpdateWallet(wallet);
        public void DeleteWallet(Wallet wallet) => WalletDAO.DeleteWallet(wallet);
    }
}
