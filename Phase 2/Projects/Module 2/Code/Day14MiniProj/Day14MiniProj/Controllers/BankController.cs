using Day14MiniProj.Context;
using Day14MiniProj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Day14MiniProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : Controller
    {
        AppDbContext _appDbContext;

        public BankController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(Customer cus)
        {
            _appDbContext.Customers.Add(cus);
            _appDbContext.SaveChanges();
            return Ok("Customer added");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddAccountDetails")]
        public IActionResult AddAccountDetails(BankCusEntry bce)
        {
            var customer = _appDbContext.Customers.Where(c => c.CusName == bce.CusName).FirstOrDefault();
            if (customer == null)
                return NotFound("Invalid customer name");
            Bank bank = new Bank() { AccNo = bce.AccNo, Amount = bce.Amount, Created_Date = bce.Created_Date, CusId = customer.CusId };
            _appDbContext.BankStore.Add(bank);
            _appDbContext.SaveChanges();
            return Ok("Account details added");
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var users = _appDbContext.BankStore.Select(c => new { c.AccNo, c.Amount, c.Created_Date }).ToList();
            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetAccountBalance")]
        public IActionResult GetAccountBalance(string Customer_name)
        {
            var balance = _appDbContext.BankStore.Where(c => c.customer.CusName == Customer_name).Select(b => new { AccountNumber = b.AccNo, BalanceAmount = b.Amount }).ToList();
            return Ok(balance);
        }
    }
}
