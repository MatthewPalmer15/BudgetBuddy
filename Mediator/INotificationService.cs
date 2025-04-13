using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Platforms;

    public interface INotificationService
    {
        public void SendNotification(string title, string content);
    }
