using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
    public class ExternalLoginDeleteModel : PageModel {
        private readonly SignInManager<ShopUser> _signInManager;
        private readonly UserManager<ShopUser> _userManager;

        public ExternalLoginDeleteModel(SignInManager<ShopUser> signInManager, UserManager<ShopUser> userManager) {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        public string IdpDisplayName { get; set; }

        public async Task<IActionResult> OnGetAsync(string idpName) {
            // Get identity provider display name
            var idps = await this._signInManager.GetExternalAuthenticationSchemesAsync();
            this.IdpDisplayName = idps.FirstOrDefault(x => x.Name.Equals(idpName))?.DisplayName;
            if (string.IsNullOrEmpty(this.IdpDisplayName)) return this.RedirectToPage("ExternalLogins");
            return this.Page();
        }

        public IActionResult OnPostCancel() {
            return this.RedirectToPage("ExternalLogins");
        }

        public async Task<IActionResult> OnPostAsync(string idpName, string idpKey) {
            var user = await this._userManager.GetUserAsync(this.User);
            var result = await this._userManager.RemoveLoginAsync(user, idpName, idpKey);
            await this._signInManager.SignInAsync(user, isPersistent: false);
            return this.RedirectToPage("ExternalLogins");
        }

    }
}