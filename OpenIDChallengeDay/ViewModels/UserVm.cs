using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDChallengeDay.ViewModels
{
    public class UserVm
    {
        public string JobTitle { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public static UserVm ToViewModel(Microsoft.Graph.User user)
        {
            var userVm = new UserVm();

            userVm.DisplayName = user.DisplayName;
            userVm.Email = user.Mail;
            userVm.JobTitle = user.JobTitle;

            return userVm;
        }
    }
}
