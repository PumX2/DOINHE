using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DOINHE_BusinessObject;
using DOINHE_Repository;

namespace DOINHE1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ODataController
    {
        private readonly IWalletRepository _walletRepository;

        public WalletController(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_walletRepository.GetAllWallets());
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var wallet = _walletRepository.GetWalletById(id);
            if (wallet == null)
                return NotFound();
            return Ok(wallet);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Wallet wallet)
        {
            _walletRepository.SaveWallet(wallet);
            return CreatedAtAction(nameof(Get), new { id = wallet.Id }, wallet);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Wallet wallet)
        {
            var existingWallet = _walletRepository.GetWalletById(id);
            if (existingWallet == null)
                return NotFound();

            _walletRepository.UpdateWallet(wallet);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var wallet = _walletRepository.GetWalletById(id);
            if (wallet == null)
                return NotFound();

            _walletRepository.DeleteWallet(wallet);
            return NoContent();
        }
    }
}
