using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.Models;

namespace Evently.Areas.Identity.Pages.Account.Manage
{
    public abstract class ManageBaseModel : PageModel
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public ManageBaseModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; private set; }

        [TempData]
        public string StatusMessage { get; set; }

        public virtual async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserName = user?.UserName;
        }
    }
} 