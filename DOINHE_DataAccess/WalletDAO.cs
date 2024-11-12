using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class WalletDAO
    {
        public static List<Wallet> GetWallets()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Wallets.ToList();
            }
        }

        public static Wallet GetWalletById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Wallets.FirstOrDefault(w => w.Id == id);
            }
        }

        public static void InsertWallet(Wallet wallet)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Wallets.Add(wallet);
                context.SaveChanges();
            }
        }

        public static void UpdateWallet(Wallet wallet)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Wallets.Update(wallet);
                context.SaveChanges();
            }
        }

        public static void DeleteWallet(Wallet wallet)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Wallets.Remove(wallet);
                context.SaveChanges();
            }
        }
    }
}
