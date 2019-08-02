## Browser Master 2.1
Browser Master 2.0 registers as your default browser and forwards web requests to your preferred browser. It works with Firefox, Chrome, Opera, Safari, Internet Explorer, and Edge.

![](https://github.com/ralphyz/browsermaster2/raw/master/images/browser_master_2.1.gif)

Browser Master 2.1 is based on [Browser Master 2.0](https://archive.codeplex.com/?p=browsermaster2) that I wrote.  That was based on the original [BrowserMaster](https://archive.codeplex.com/?p=browsermaster) written for Windows XP by jquintus. It has been updated to work with Windows 7.

Browser Master registers as your default browser and captures web requests.  The user is briefly presented with a choice of which browser to use for the web request.  If no choice is made (and no key is pressed) before the timeout, it forwards the web request to your preferred browser.

###Install
1. Extract the icons directory and BrowserMaster.exe into the same directory.
2. Register BrowserMaster.exe as a web browser:
	*	Open a Command Prompt (run as Administrator since it must change the registry)
	*	Change to the directory that contains BrowserMaster.exe
	*	Run `BrowserMaster.exe -r`
	*	Running BrowserMaster for the first time will create BrowserMaster.xml

###Removal
1. Set another web browser  as your default browser.
2. Delete the icons directory, BrowserMaster.exe, and BrowserMaster.xml

###BrowserMaster.xml
*	The browser-selection timeout can be changed longer or shorter by editing:
```xml
<DelayInSeconds>2</DelayInSeconds>
```
*	The browser display order can be changed by re-ordering the Browser Config sections inside the XML file:
```xml
<BrowserConfig Name...>...</BrowserConfig>
```
The first BrowserConfig is the default (selected when the timeout has been reached)

*	To disable a browser, set` Enabled="false"` in the BrowserConfig tag
```xml
<BrowserConfig Name="Edge" Enabled="false">
```

###Usage
![](https://github.com/ralphyz/browsermaster2/raw/master/images/config.png)

Any link you click, URL shortcut you double-click, or website you type into the Windows Run box will launch BrowserMaster.  If you do not make a selection by the timeout period (2 seconds default), the first browser will be launched for you.  If you press an arrow key (left or right), that timeout goes away.  BrowserMaster will stay up indefinitely at that point.  Press Enter to choose a browser, or press ESC to cancel.  If you are fast enough (or set a very long timeout), you may click an icon to select a browser.

BrowserMaster automatically checks for which browsers you have installed (out of the supported browsers).  Right now, it assumes you have installed them in the Program Files or Program Files (x86) directories.  If you have them installed somewhere else, you can manually edit the BrowserMaster.xml file and add the browser.  Other browsers (like Brave) can be used if you manually add them and copy the config from another browser.  Chrome would be a good option to copy for Brave.

