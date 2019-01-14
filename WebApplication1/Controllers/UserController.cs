using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        IEmailSender emailSender;

        public UsersController(IEmailSender emailSender)
        {
            this.emailSender = emailSender; 
        }

        public IActionResult Index([FromServices] UserManager<ApplicationUser> userManager)
        {
            return View(userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Edit(string id, [FromServices] ApplicationDbContext db)
        {
            if (id==null)
            {
                return View(new ApplicationUser());
            }
            else
            {
                return View(db.ApplicationUser.Find(id));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user, [FromServices] ApplicationDbContext db, [FromServices] UserManager<ApplicationUser> userManager)
        {
            var user_v_base = db.ApplicationUser.Find(user.Id);
            if (user_v_base==null)
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.EmailConfirmed = true;
                user.UserName = user.Email;
                await userManager.CreateAsync(user);
                var urlEncode = System.Web.HttpUtility.UrlEncode(await userManager.GeneratePasswordResetTokenAsync(user));
                var callbackUrl = $"{Request.Scheme}://{Request.Host.Value}/Identity/Account/ResetPassword?userId={user.Id}&code={urlEncode}";
                await emailSender.SendEmailAsync(user.Email, "Ссылка", callbackUrl);
                await db.SaveChangesAsync(); 
                return RedirectToAction("Index");
            }

            user_v_base.Imya = user.Imya;
            user_v_base.Familiya = user.Familiya;
            user_v_base.Otchestvo = user.Otchestvo; 
            user_v_base.Email = user.Email;
            user_v_base.UserName = user.Email;

            await userManager.UpdateAsync(user_v_base);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string id, [FromServices] UserManager<ApplicationUser> userManager)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleAdmin(string id, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var user = await userManager.FindByIdAsync(id);
            var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
            }
            return RedirectToAction("Index");
        }
    }
}