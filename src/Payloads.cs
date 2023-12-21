namespace NemligSharp;

internal record LoginPayload(string Username, string Password, bool AutoLogin = false, bool CheckForExistingProducts = true, bool DoMerge = true, bool AppInstalled = false);