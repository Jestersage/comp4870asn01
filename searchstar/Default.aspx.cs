using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{

    protected int fileNumber;
    protected int totalNumberOfFile;
    protected List<string> results;

    protected void Page_Load(object sender, EventArgs e)
    {
 
        tbResultFileName.Attributes.Add("readonly", "readonly");
        tbResultFileNumber.Attributes.Add("readonly", "readonly");
        tbResult.Attributes.Add("readonly", "readonly");

        if (IsPostBack)
        {
            fileNumber = Convert.ToInt32(ViewState["fileNumber"]);
            totalNumberOfFile = Convert.ToInt32(ViewState["totalNumberOfFile"]);            
            results = (List<string>)ViewState["results"];
            
        }
        else
        {
            fileNumber = 0;
            totalNumberOfFile = 0;
            results = new List<string>();
        }
    }

    /// <summary>
    /// checks each file in the dir one by one to see if it contains all the words 
    /// in the search textbox when user click search button
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        fileNumber = 0;
        totalNumberOfFile = 0;
        results = new List<string>();

        string dir = Server.MapPath("~") + "\\" + "files";
        string[] allFiles = Directory.GetFiles(dir);


        foreach (string filename in allFiles)
        {
            if (IsContain(filename))
                results.Add(filename);
        }
        totalNumberOfFile = results.Count();
        if (results != null && totalNumberOfFile > 0)
        {
            tbResultFileName.Text = Path.GetFileName(results[0]);
            StreamReader reader = new StreamReader(results[0], System.Text.Encoding.Default, true);
            tbResult.Text = reader.ReadToEnd();
            tbResultFileNumber.Text = "Result " + (fileNumber + 1) + " of " + totalNumberOfFile;

        }
        else
        {
            tbResultFileName.Text = "file not found";
            tbResult.Text = "";
            tbResultFileNumber.Text = "No Results";
        }
        ViewState["fileNumber"] = fileNumber;
        ViewState["totalNumberOfFile"] = totalNumberOfFile;
        ViewState["results"] = results;

    }

    /// <summary>
    /// helper method to see if the file contains all the searching key words
    /// </summary>
    /// <param name="filename">the full path of a file including its filename</param>
    /// <returns>a bool valude indicates if the file contains all the searching words</returns>
    private bool IsContain(string filename)
    {
        string input = tbSearch.Text;
        List<string> keys = MakeKeyList(input);
        CultureInfo culture = new CultureInfo("en-US");
        string fileContent = File.ReadAllText(filename);
        foreach (string key in keys)
        {
            //if (!fileContent.Contains(key))
            if (culture.CompareInfo.IndexOf(fileContent, key, CompareOptions.IgnoreCase)<0)
                return false;
        }
        return true;
    }

    /// <summary>
    /// go to previous file in the results list
    /// </summary>
    protected void previous_click(object sender, ImageClickEventArgs e)
    {
        if (fileNumber > 0)
            fileNumber--;
        if (results != null && totalNumberOfFile > 0)
        {
            tbResultFileName.Text = Path.GetFileName(results[fileNumber]);
            StreamReader reader = new StreamReader(results[fileNumber], System.Text.Encoding.Default, true);
            tbResult.Text = reader.ReadToEnd();
            tbResultFileNumber.Text = "Result " + (fileNumber + 1) + " of " + totalNumberOfFile;
        }
        else
        {
            tbResultFileName.Text = "file not found";
            tbResult.Text = "";
            tbResultFileNumber.Text = "No Results";
        }
        ViewState["fileNumber"] = fileNumber;
        ViewState["totalNumberOfFile"] = totalNumberOfFile;
        ViewState["results"] = results;

    }

    /// <summary>
    /// go to next file in the results list
    /// </summary>
    protected void next_click(object sender, ImageClickEventArgs e)
    {
        if (fileNumber < totalNumberOfFile - 1 && totalNumberOfFile > 0)
        {
            fileNumber++;
        }
        if (results != null && totalNumberOfFile > 0)
        {
            tbResultFileName.Text = Path.GetFileName(results[fileNumber]);
            StreamReader reader = new StreamReader(results[fileNumber], System.Text.Encoding.Default, true);
            tbResult.Text = reader.ReadToEnd();
            tbResultFileNumber.Text = "Result " + (fileNumber + 1) + " of " + totalNumberOfFile;
        }
        else
        {
            tbResultFileName.Text = "file not found";
            tbResult.Text = "";
            tbResultFileNumber.Text = "No Results";

        }
        ViewState["fileNumber"] = fileNumber;
        ViewState["totalNumberOfFile"] = totalNumberOfFile;
        ViewState["results"] = results;
    }

    /// <summary>
    /// go to the first file in the results list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void goFirst_Click(object sender, ImageClickEventArgs e)
    {
        fileNumber = 0;
        if (results != null && totalNumberOfFile > 0)
        {
            tbResultFileName.Text = Path.GetFileName(results[fileNumber]);
            StreamReader reader = new StreamReader(results[fileNumber], System.Text.Encoding.Default, true);
            tbResult.Text = reader.ReadToEnd();
            tbResultFileNumber.Text = "Result " + (fileNumber + 1) + " of " + totalNumberOfFile;

        }
        else
        {
            tbResultFileName.Text = "file not found";
            tbResult.Text = "";
            tbResultFileNumber.Text = "No Results";

        }
        ViewState["fileNumber"] = fileNumber;
        ViewState["totalNumberOfFile"] = totalNumberOfFile;
        ViewState["results"] = results;
    }

    /// <summary>
    /// go to the last file in the results list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void goLast_Click(object sender, ImageClickEventArgs e)
    {
        if (totalNumberOfFile > 0)
            fileNumber = totalNumberOfFile - 1;

        if (results != null && totalNumberOfFile > 0)
        {
            tbResultFileName.Text = Path.GetFileName(results[fileNumber]);
            StreamReader reader = new StreamReader(results[fileNumber], System.Text.Encoding.Default, true);
            tbResult.Text = reader.ReadToEnd();
            tbResultFileNumber.Text = "Result " + (fileNumber + 1) + " of " + totalNumberOfFile;
        }
        else
        {
            tbResultFileName.Text = "file not found";
            tbResult.Text = "";
            tbResultFileNumber.Text = "No Results";

        }
        ViewState["fileNumber"] = fileNumber;
        ViewState["totalNumberOfFile"] = totalNumberOfFile;
        ViewState["results"] = results;

    }

    /// <summary>
    /// Find the search key list, after delimiting and removal of excluded words
    /// </summary>
    /// <param name="input">the input keys</param>
    /// <returns></returns>
    private List<string> MakeKeyList(string input)
    {
        String delims = "[.,?!\\s]+";

        String filename = "~/exclusion/exclusion.txt";

        List<string> excludeList = new List<string>();

        string line;

        // Read the file and display it line by line.
        //https://msdn.microsoft.com/en-us/library/dd383503(v=vs.110).aspx
        System.IO.StreamReader file =
            new System.IO.StreamReader(Server.MapPath(filename));
        while ((line = file.ReadLine()) != null)
        {
            excludeList.Add(line);
        }

        file.Close();

        //List<String> keyList = input.Split(' ').ToList();
        List<String> keyList = Regex.Split(input, delims).ToList();

        foreach (string excludeWord in excludeList)
        {
            keyList.RemoveAll(n => n.Equals(excludeWord, StringComparison.OrdinalIgnoreCase));
        }
        return keyList;
    }

    /// <summary>
    /// save the file: shows up a window to prompt user to choose the directory for saving the file to
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, ImageClickEventArgs e)
    {
        if (results != null && totalNumberOfFile > 0)
        {
            // http://www.c-sharpcorner.com/uploadfile/afenster/how-to-download-a-file-in-Asp-Net/
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(results[fileNumber]));

            // Write the file to the Response
            const int bufferLength = 10000;
            byte[] buffer = new Byte[bufferLength];
            int length = 0;
            Stream download = null;
            string fileName = "./files/" + Path.GetFileName(results[fileNumber]);
            try
            {
                download = new FileStream(Server.MapPath(fileName),
                                                               FileMode.Open,
                                                               FileAccess.Read);
                do
                {
                    if (Response.IsClientConnected)
                    {
                        length = download.Read(buffer, 0, bufferLength);
                        Response.OutputStream.Write(buffer, 0, length);
                        buffer = new Byte[bufferLength];
                    }
                    else
                    {
                        length = -1;
                    }
                }
                while (length > 0);
                Response.Flush();
                Response.End();
            }
            finally
            {
                if (download != null)
                    download.Close();
            }
        
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "File not Found. Unable to save." + "');", true);
        }

    }

}