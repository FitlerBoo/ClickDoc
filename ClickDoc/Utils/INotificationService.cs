﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickDoc.Utils
{
    public interface INotificationService
    {
        void ShowSuccess(string message);
        void ShowError(string message);
        void ShowWarning(string message);
    }
}
