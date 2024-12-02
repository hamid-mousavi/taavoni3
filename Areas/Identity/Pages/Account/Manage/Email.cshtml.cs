using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace taavoni3.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles ="Admin")]
    public class Email : PageModel
    {
        private readonly ILogger<Email> _logger;

        public Email(ILogger<Email> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}