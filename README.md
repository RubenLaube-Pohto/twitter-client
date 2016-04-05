# Twitter Client

Twitter Client by Ruben Laube-Pohto using .NET and TweetSharp.

[Introduction](#introduction)  
[Requirements](#requirements)  
[Issues](#issues)  
[Spent Time](#spent-time)

## Introduction

The purpose of this project was to create a simple client program for Twitter using the Twitter API. With the finished software the user should be able to login, get tweets, send tweets and browse the sevice in a similiar way as on the website.

## Requirements

[TweetSharp](https://github.com/danielcrenna/tweetsharp) 2.3.1

Installation with NuGet Package Manager Console [[1]]:

	Install-Package TweetSharp -Version 2.3.1

In order to authorize the app on Twitter it was required to register the app. This provided the keys required to connect to the API with OAuth. The keys were stored in App.config which is not included in this repo because the keys were considered sensitive information.

## Issues

### Change Twitter Wrapper

Twitterizer seemed too complicated so TweetrSharp was chosen instead.

## Spent Time

| Date | Hours | Task |
| :---: | :---: | :---: |
| 18.3.2016 | 2 | Setup project and gather information |
| 30.3.2016 | 2 | Integrate Twitterizer |
| 31.3.2016 | 5 | Change to TweetSharp. Add basic login and get timeline |
| 4.4.2016 | 3 | Implement login pin checking and save credentials |
| Total hours: | 12 |  |

[1]: https://www.nuget.org/packages/TweetSharp/
