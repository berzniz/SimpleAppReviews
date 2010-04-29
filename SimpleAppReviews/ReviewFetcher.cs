using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace SimpleAppReviews
{
    class ReviewFetcher
    {
        WebClient webclient = new WebClient();

        public LinkedList<ReviewEntry> fetch(string storeId, string appId)
        {
            LinkedList<ReviewEntry> reviews = new LinkedList<ReviewEntry>();
            
            string reviewsURLString = "http://ax.phobos.apple.com.edgesuite.net/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=" + appId + "&pageNumber=0&sortOrdering=4&type=Purple+Software";
            string storeFront = storeId + "-1";
            webclient.Headers.Remove("X-Apple-Store-Front");
            webclient.Headers.Remove("User-Agent");
            webclient.Headers.Add("X-Apple-Store-Front",    storeFront);
            webclient.Headers.Add("User-Agent",             "iTunes/4.2 (Macintosh; U; PPC Mac OS X 10.2)");
            byte[] responseBytes = webclient.DownloadData(reviewsURLString);
            string response = Encoding.UTF8.GetString(responseBytes);

            int curIndex    = 0;
            int stopIndex   = 0;

            while (true)
            {
                ReviewEntry curReview = new ReviewEntry();
                curReview.storeId = storeId;

                curIndex = response.IndexOf("<TextView topInset=\"0\" truncation=\"right\" leftInset=\"0\" squishiness=\"1\" styleSet=\"basic13\" textJust=\"left\" maxLines=\"1\">", curIndex);
                if (curIndex < 0 || curIndex >= response.Length)
                    break;

                curIndex = response.IndexOf("<b>", curIndex) + 3;
                stopIndex = response.IndexOf("</b>", curIndex);
                curReview.reviewTitle = response.Substring(curIndex, stopIndex - curIndex).Trim();
                curReview.reviewTitle = HttpUtility.HtmlDecode(curReview.reviewTitle);

                curIndex = response.IndexOf("<HBoxView topInset=\"1\" alt=\"", curIndex) + 28;
                stopIndex = response.IndexOf(" ", curIndex);
                curReview.reviewStars = response.Substring(curIndex, stopIndex - curIndex).Trim();

                curIndex = response.IndexOf("<b>", curIndex) + 3;
                curIndex = response.IndexOf("<b>", curIndex) + 3;
                stopIndex = response.IndexOf("</b>", curIndex);
                curReview.reviewUser = response.Substring(curIndex, stopIndex - curIndex).Trim();
                curReview.reviewUser = HttpUtility.HtmlDecode(curReview.reviewUser);

                stopIndex = response.IndexOf("</SetFontStyle>", curIndex);
                string reviewDateAndVersion = response.Substring(curIndex, stopIndex - curIndex);
                reviewDateAndVersion.Replace("\n", "");
                string[] dateVersionSplitted = reviewDateAndVersion.Split(new string[] {"- "}, StringSplitOptions.None);
                if (dateVersionSplitted.Count() == 3)
                {
                    curReview.reviewVersion = dateVersionSplitted[1].Trim();
                    curReview.reviewDate = dateVersionSplitted[2].Trim();
                }

                curIndex = response.IndexOf("<SetFontStyle normalStyle=\"textColor\">", curIndex) + 38;
                stopIndex = response.IndexOf("</SetFontStyle>", curIndex);
                curReview.reviewText = response.Substring(curIndex, stopIndex - curIndex).Trim();
                curReview.reviewText = HttpUtility.HtmlDecode(curReview.reviewText);

                reviews.AddLast(curReview);
            }                 

            return reviews;
        }
        
    }
}
