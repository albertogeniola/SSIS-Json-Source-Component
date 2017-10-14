# SSIS-Json-Source-Component ![Logo](https://www.hardwareforyou.it/images/loghi/256x256.png)

# What's JSONSource?

JSONSource is a custom source component developed for Microsoft Integration Services, compatible with MS SQL Server 2012/2014/**2016**. The goal of this component is to allow the developer to integrate JSON datasources into his ETL projects.
Among the major features of JSONSource, there are:

*   Read JSON Data directly from a webservice via HTTP GET/POST/PUT/DELETE
*   Read JSON Data from a URL coded into a project variable or hard coded
*   Read JSON Data from file or from a file pointed by a project variable or hard coded
*   Given a mapping table, JSONSource maps every attribute of the json objects into the corresponding output field of the given table.
*   Supports complex jsonpath queries
*   Supports HTTP GET/POST parameters via input lines/variables/hard-coding
*   Login into webservice are now possible using a combination of multiple components

## Flexible source

JSONSource supports different kinds of data sources: it can be either used for parsing a text file on the disk or directly download the json file, by performing. Beside, the developer can specify a variable holding the HTTP URL or the physical disk path to the json data.

When dealing with webservices or standard http pages, json source is capable of executing the most common HTTP requests type, such as GET/POST/PUT/DELETE. Moreover HTTP requests parameters are supported, so the developer can perform complex actions with a webservice. Login via access tokes are then supported, simply perform a login request and save the token into a variable, then perform next queries using that variable as query parameter!

## Easy Mapping and advanced json parsing

The mapping table is a grid that defines which object fields to take into account and how to interpret them: for each object-field the developer has to specify an output column name and an output data type. Although this component supports both array of objects and simple objects, some more complex operation is possible thanks to the magic of JsonPath. Indeed, the developer can specify very complex query to get almost any combination of data out of the datasource!

## Easy Instalation

To install this component the developer can easily use the custom installer, which will automatically detect every compatible MSSQL installation and will ask where to install the feature. However, the stand-alone dll is also given, for expert users.

## Do you have any idea?

I'm continuously listening to users feedbacks. The following are some points I want to implement in future releases of the software. Due to small time I have to dedicate to this project I cannot state precisely when those features will be implemented. However, if you have any other idea or if you want to tell which feature would be better to implement first, feel free to vote it on the discussion section.

**ALREADY IMPLEMENTED**

*   Supporting POST actions alongside GET **DONE with 1.0.200**
*   Support parsing single JSON object instead of whole arrays **DONE with 1.0.200**
*   Authentication token management for most common auth protocols **DONE with 1.0.200**
*   Input lane HTTP parameters support **DONE in beta 1.1.00**
*   Batch query run **DONE in beta 1.1.00**
*   Testing UI for faster debugging **DONE in beta 1.1.00**
*   Add support for MS SQL2016 **DONE in beta 1.1.01**
*   Fix Json DateTime parsing** **DONE in beta 1.1.01****
*   Add support automatic metadata upgrade **DONE in beta 1.1.01**
*   Custom HTTP header support **DONE in beta 1.1.01**

****PROGRAMMED FOR NEXT RELEASE****

*   Support URL derived from input lanes **DONE in current beta!**
*   Add more control over errors, error lane/make the component fail if no line is outputted **DONE in current beta!**
*   Retrying on connection error **DONE in current beta!**

**PROGRAMMED NEXT...**

*   Improve documentation, provide some videos and tutorials
*   Add automated tests for high code quality
*   Add proxy configuration support
*   Improve JSON Filter capabilities
*   Auto detection of data-model
*   High Level OAuth support

**If some of these upcoming features are of your interest, open a discussion so I can add it as work item for future release. The more people let me know what they need, the better I can prioritize features implementation.**

# What if I need it FAST?

Basically my time is limited. However you may contact me in private and I might be able to help you somehow. Of course donation may be a great incentive to speed up feature implementation!

# Buy me a beer

![](http://blog.mlive.com/kalamabrew/2008/03/small_Beer%20Mug%20Icon.jpg)I'm an university student, attending my last year in Computer Science. My time is pretty limited due to the amount of work I face for writing my thesis. I usually code on Friday-Saturday nights, when nobody is up for a beer. So, if I've saved your time and you like the component, why don't buy me a beer? As a developer you know how long it takes to get a stable and working product, so buying a beer would be a great way to thank me!

[![Make a donation!](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif "Make a donation!")](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=6HPAB89UYSZF2 "Donate")

# Wanna get in contact with me? 

At the moment I am finishing my studies, but I am totally open to job offer or part-time collaborations. So far I've got in touch with many different aspects of IT: that makes me a very versatile asset. Why don't you have a look at my LI profile?

## [![](https://static.licdn.com/sc/h/3m4lyvbs6efg8pyhv7kupo6dh)](https://www.linkedin.com/in/albertogeniola "LinkedIn Profile")[ ](https://www.linkedin.com/in/albertogeniola "LinkedIn Profile")[Alberto Geniola](https://www.linkedin.com/in/albertogeniola "LinkedIn Profile")

# !New Beta is OUT! SQL2016 Support added!

### The new beta version (1.1.000) implements many interesting features requested by the users who got in touch with me! Have a look at the screen below! Having more ideas? Let me know by opening a discussion! 

[![](http://download-codeplex.sec.s-msft.com/Download?ProjectName=jsonsource&DownloadId=1560332)](http://download-codeplex.sec.s-msft.com/Download?ProjectName=jsonsource&DownloadId=1560332)

# Rate the component

If you are using this component and you like it, please rate it so others will try it as well. Your opinion is important for me and for other users!

![](https://www.google-analytics.com/__utm.gif?utmwv=1&utmn=93757836&utmsr=-&utmsc=-&utmul=-&utmje=0&utmfl=-&utmdt=-&utmhn=jsonsource.codeplex.com&utmr=&utmp=&utmac=UA-10387182-3)
