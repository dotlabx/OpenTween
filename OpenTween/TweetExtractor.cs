﻿// OpenTween - Client of Twitter
// Copyright (c) 2015 kim_upsilon (@kim_upsilon) <https://upsilo.net/~upsilon/>
// All rights reserved.
//
// This file is part of OpenTween.
//
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
// for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenTween
{
    public static class TweetExtractor
    {
        /// <summary>
        /// テキストから URL を抽出して返します
        /// </summary>
        public static IEnumerable<string> ExtractUrls(string text)
        {
            var urlMatches = Regex.Matches(text, Twitter.rgUrl, RegexOptions.IgnoreCase).Cast<Match>();
            foreach (var m in urlMatches)
            {
                var before = m.Groups["before"].Value;
                var url = m.Groups["url"].Value;
                var protocol = m.Groups["protocol"].Value;
                var domain = m.Groups["domain"].Value;
                var path = m.Groups["path"].Value;
                if (protocol.Length == 0)
                {
                    if (Regex.IsMatch(before, Twitter.url_invalid_without_protocol_preceding_chars))
                        continue;

                    var validUrl = false;
                    string lasturl = null;

                    var last_url_invalid_match = false;
                    var domainMatches = Regex.Matches(domain, Twitter.url_valid_ascii_domain, RegexOptions.IgnoreCase).Cast<Match>();
                    foreach (var mm in domainMatches)
                    {
                        lasturl = mm.Value;
                        last_url_invalid_match = Regex.IsMatch(lasturl, Twitter.url_invalid_short_domain, RegexOptions.IgnoreCase);
                        if (!last_url_invalid_match)
                        {
                            validUrl = true;
                        }
                    }

                    if (last_url_invalid_match && path.Length != 0)
                    {
                        validUrl = true;
                    }

                    if (validUrl)
                    {
                        yield return url;
                    }
                }
                else
                {
                    yield return url;
                }
            }
        }
    }
}