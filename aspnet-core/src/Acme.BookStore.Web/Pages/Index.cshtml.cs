using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using Volo.Abp.Users;

namespace Acme.BookStore.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICurrentUser _currentUser;

        public IndexModel(ILogger<IndexModel> logger, ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        public void OnGet()
        {
            Debug.WriteLine(_currentUser.IsAuthenticated); // false here.
        }
    }
}