# Helyn logger — Prototype

This is the first attempt at making a custom logger for Unity.

## Status
Prototype — development abandoned. This repository contains exploratory work using Microsoft.Extensions.Logging, it has not been tested and comes with no warranty. The project is kept for reference.

## Main goal
The main features I wanted for this logger were:
- to filter logs using namespace, class name, and log level
- to have a custom log file
- to have custom log formats for console and log file

## Where it failed
I had seen that the Microsoft.Extensions.Logging classes already implement all of those features. That's the main reason I chose to implement my logger with them (I was also curious about the framework).
Problems arose when I tried using it, mainly:
- I had in mind that C++ macros could be declared to simplify code; for example: `#define HELYN_LOGGABLE private static readonly ILogger helynLogger = LoggerSetup.LoggerFactory.CreateLogger();`. I did not find a similar approach in C# that would allow a simple way to use my logger.
- Even if I could make it easy to declare the logger, it would still mean using a custom logger instead of the default Unity logging classes. Moreover, it would not catch and filter third-party code I might be using.

## Conclusion
In the end, it appears that using the default Unity logging classes is also a feature I want my logger to have. That's why I'll start over using the Unity logging classes.

However, since I learned some interesting things about using Microsoft.Extensions.Logging classes, I'll still keep this repository around.

## Documentation
- [Technical documentation](Doc/Technical.md)
- [User documentation](Doc/User.md)
