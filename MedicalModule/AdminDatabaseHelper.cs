using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace MedicalDataManagement.MedicalModule;

/// <summary>
/// Helper class for Oracle database operations.
/// </summary>
public static class AdminDatabaseHelper
{
    /// <summary>
    /// Verifies that the Oracle database is reachable and operational.
    /// If connection fails, displays an error message to the user.
    /// </summary>
    /// <returns>True if connection is successful; otherwise, false.</returns>
    public static bool CheckConnection(string username, string password, bool showError = true)
    {
        try
        {
            using (OracleConnection connection = new OracleConnection(AdminDatabaseService.BuildConnectionString(username, password)))
            {
                // Attempt to open the connection to verify availability
                connection.Open();
                
                // Connection successful, no message needed
                return true;
            }
        }
        catch (OracleException ex)
        {
            // Specifically handling Oracle-related connection errors
            if (showError) DisplayConnectionError(ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            // Handling any other potential exceptions during connection attempt
            if (showError) DisplayConnectionError(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Displays a standardized error message to the user when the database is unreachable.
    /// </summary>
    /// <param name="errorMessage">Detailed error message from the exception.</param>
    private static void DisplayConnectionError(string errorMessage)
    {
        MessageBox.Show(
            "Cannot connect to the database. Please check if the Oracle service is running.\n\n" +
            "Details: " + errorMessage,
            "Database Connection Failed",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}

