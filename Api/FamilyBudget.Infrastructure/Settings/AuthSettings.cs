﻿namespace FamilyBudget.Infrastructure.Settings;

public class AuthSettings
{
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
}