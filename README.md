# Twitter Client

Twitter Client by Ruben Laube-Pohto using .NET and TweetSharp.

[Introduction](#introduction)  
[Requirements](#requirements)  
[Issues](#issues)  
[Closing Report](#closing-report)  
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

### Refreshing user tokens

At the moment Twitter does not expire user tokens. The token becomes unusable if the user rejects the application from their settings.

## Closing Report

### Installation

The compiled application doesn't need to be installed and can be used as is with the required libraries. The application was tested on Windows 8 & 10.

### About the application

The final application allows the user to
- login
- get tweet from home timeline
- get own tweets
- send basic tweets

Many finer aspects of the Twitter service had to be left out due to time constraints.

### Screenshots

To be added.

### Further development

There are no plans to do further development on this project.

### What was learned

- Usage of NuGet
- Twitter API
- Using external libraries in .NET

### Expected points

The application is a bit minimalistic and there was not that much time spent. However, that which was done, was done well. Points expected: 15 / 30.

## Spent Time

| Date | Hours | Task |
| :---: | :---: | :---: |
| 18.03.2016 | 2 | Setup project and gather information |
| 30.03.2016 | 2 | Integrate Twitterizer |
| 31.03.2016 | 5 | Change to TweetSharp. Add basic login and get timeline |
| 04.04.2016 | 3 | Implement login pin checking and save credentials |
| 13.04.2016 | 5 | Add final implementations and documentation |
| Total hours: | 17 |  |

[1]: https://www.nuget.org/packages/TweetSharp/
