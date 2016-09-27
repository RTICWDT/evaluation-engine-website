/*
 * Tests the basic functionality of the Account Manager, which would otherwise require rewriting of the 
 * AccountController. Built based off of the PIMS tests.
 * 
 */

var x = require('casper').selectXPath;

var casper = require('casper').create({
    clientScripts: "jquery-1.8.3.min.js",
    pageSettings: {
        userAgent: 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.10 (KHTML, like Gecko) Chrome/23.0.1262.0 Safari/537.10'
    },
    viewportSize: {
        width: 1024,
        height: 768
    },
});

var  baseUrl    =  "http://evaluationenginedevelopment.mprinc.com/";
//var  baseUrl    = "http://localhost:50217/";
var  basePath   =  "test_files/";
var  imagesPath =  basePath + "images/";
var  downloads  =  basePath + "downloads/";
var  fixtures   =  basePath + "fixtures/";
var  fileUpload =  "";
var  userEmail  =  "smithale@gmail.com";
var  userPwd    =  "Wordypass1212!";
//var  userPwdNew =  "P4$$w0rd1";
var  badPassword = "badPassword";

var  userToLock = "smithale@gmail.com";
var  userToLockPwd = "P4$$w0rd";
//var  testData   =  require('./Fixtures').create().getDATE();
//var  sectionTitle = "DATE Program Management";

// Check if any arguments were passed.
if(casper.cli.has("none"))
{

} else {

}

casper.start(baseUrl + "Account/LogOn",
    function () {
	this.echo("##teamcity[testSuiteStarted name='Account Manager Functionality']");
    }
);

// Login to the site
require('./EELogin').create(casper, userEmail, userPwd); 

casper.thenOpen(baseUrl + "Account/MyAccount").then(function() {
    this.echo(baseUrl + "Account/MyAccount");
});

/*casper.waitForSelector("form input[name='OldPassword']",
    function success() {
        this.test.assertExists("form input[name='OldPassword']");
        this.click("form input[name='OldPassword']");
		this.capture(imagesPath + 'PW_Change_1.png');
    },
    function fail() {
        this.test.assertExists("form input[name='OldPassword']");
		this.capture(imagesPath + 'PW_Change_fail.png');
});
casper.waitForSelector("form input[name='OldPassword']",
    function success() {
        this.fill("table", {"OldPassword": userPwd});
		this.capture(imagesPath + 'PW_Change_2.png');
    },
    function fail() {
        this.test.assertExists("form");
		this.capture(imagesPath + 'PW_Change_2_fail.png');
});
casper.waitForSelector("form input[name='Password']",
    function success() {
        this.fill("table", {"Password": userPwdNew});
		this.capture(imagesPath + 'PW_Change_3.png');
    },
    function fail() {
        this.test.assertExists("form");
		this.capture(imagesPath + 'PW_Change_3_fail.png');
});
casper.waitForSelector("form",
    function success() {
        this.fill("table", {"ConfirmPassword": userPwdNew});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form input[type=submit][value='Change Password']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Change Password']");
        this.click("form input[type=submit][value='Change Password']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Change Password']");
});

casper.then(
	function() {
		this.test.assertExists("img[src='/Content/inc/img/icon_check.png']");
		this.test.assertTextExists("Your password has been updated.");
});

casper.waitForSelector("form input[name='OldPassword']",
    function success() {
        this.test.assertExists("form input[name='OldPassword']");
        this.click("form input[name='OldPassword']");
    },
    function fail() {
        this.test.assertExists("form input[name='OldPassword']");
});
casper.waitForSelector("form",
    function success() {
        this.fill("table", {"OldPassword": userPwdNew});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form",
    function success() {
        this.fill("table", {"Password": userPwd});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form",
    function success() {
        this.fill("table", {"ConfirmPassword": userPwd});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form input[type=submit][value='Change Password']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Change Password']");
        this.click("form input[type=submit][value='Change Password']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Change Password']");
});

casper.then(
	function() {
		this.test.assertExists("img[src='/Content/inc/img/icon_check.png']");
		this.test.assertTextExists("Your password has been updated.");
});*/

casper.thenOpen(baseUrl + "Account/Users").then(function() {
    this.echo(baseUrl + "Account/Users");
});

casper.waitForSelector("a[href='/Account/Edit?userName=alexsmith%40rti.org']",
    function success() {
        this.test.assertExists("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
        this.click("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
    },
    function fail() {
        this.test.assertExists("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
});

casper.waitForSelector("form input[name='Organization']",
    function success() {
        this.test.assertExists("form input[name='Organization']");
        this.click("form input[name='Organization']");
		this.capture(imagesPath + 'Org_success.png');
    },
    function fail() {
        this.test.assertExists("form input[name='Organization']");
		this.capture(imagesPath + 'Org_fail.png');
});

casper.waitForSelector("form",
    function success() {
        this.fill("form", {"Organization": ""});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form input[type=submit][value='Save Changes']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
        this.click("form input[type=submit][value='Save Changes']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
});

casper.waitForText("The Organization field is required.",
    function success() {
        this.test.assertTextExists("The Organization field is required.");
    },
    function fail() {
        this.test.assertTextExists("The Organization field is required.");
});


casper.waitForSelector("form input[name='Organization']",
    function success() {
        this.test.assertExists("form input[name='Organization']");
        this.click("form input[name='Organization']");
    },
    function fail() {
        this.test.assertExists("form input[name='Organization']");
});

casper.waitForSelector("form",
    function success() {
        this.fill("form", {"Organization": "RTI"});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form input[type=submit][value='Save Changes']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
        this.click("form input[type=submit][value='Save Changes']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
});
// submit form
casper.waitForSelector("a[href='/Account/Edit?userName=alexsmith%40rti.org']",
    function success() {
        this.test.assertExists("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
        this.click("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
    },
    function fail() {
        this.test.assertExists("a[href='/Account/Edit?userName=alexsmith%40rti.org']");
});
casper.waitForSelector("form input[name='Organization']",
    function success() {
        this.test.assertExists("form input[name='Organization']");
        this.click("form input[name='Organization']");
    },
    function fail() {
        this.test.assertExists("form input[name='Organization']");
});
casper.waitForSelector("form",
    function success() {
        this.fill("form", {"Organization": "MPR"});
    },
    function fail() {
        this.test.assertExists("form");
});
casper.waitForSelector("form input[type=submit][value='Save Changes']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
        this.click("form input[type=submit][value='Save Changes']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
});

casper.thenOpen(baseUrl + "Account/LogOff").then(function() {
    this.echo(baseUrl + "Account/LogOff");
});

//start locking test

casper.thenOpen(baseUrl + "Account/LogOn").then(function() {
    this.echo(baseUrl + "Account/LogOn");
});

//locks the account
/*require('./EELockAccount').create(casper, userToLock);

require('./EELogin').create(casper, userEmail, userPwd); 

casper.thenOpen(baseUrl + "Account/Users").then(function() {
    this.echo(baseUrl + "Account/Users");
});

casper.waitForSelector("a[href='/Account/Edit?userName=smithale%40gmail.com']",
    function success() {
        this.test.assertExists("a[href='/Account/Edit?userName=smithale%40gmail.com']");
        this.click("a[href='/Account/Edit?userName=smithale%40gmail.com']");
    },
    function fail() {
        this.test.assertExists("a[href='/Account/Edit?userName=smithale%40gmail.com']");
});
casper.waitForSelector("form input[name='IsLocked']",
    function success() {
        this.test.assertEquals(this.getElementAttribute("form input[name='IsLocked'][value='True']", "checked"), 'checked');
        this.click("form input[name='IsLocked'][value='True']");
    },
    function fail() {
        this.test.assertExists("form input[name='IsLocked'][value='True']");
		this.capture(imagesPath + 'account_locked_fail.png');
});
casper.waitForSelector("form input[type=submit][value='Save Changes']",
    function success() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
        this.click("form input[type=submit][value='Save Changes']");
    },
    function fail() {
        this.test.assertExists("form input[type=submit][value='Save Changes']");
});

casper.thenOpen(baseUrl + "Account/LogOff").then(function() {
    this.echo(baseUrl + "Account/LogOff");
});

//require('./EELogin').create(casper, userToLock, userPwd); 

casper.thenOpen(baseUrl + "Account/Edit?userName=smithale%40gmail.com").then(function() {
    this.echo(baseUrl + "Account/Edit?userName=smithale%40gmail.com");
});

casper.waitForSelector("form input[name='IsLocked']",
    function success() {
        this.test.assertEquals("form input[name='IsLocked'][value='True']", 'checked');
    },
    function fail() {

});
*/
// End of test suite
casper.then(
    function () {
	this.echo("##teamcity[testSuiteFinished name='Account Manager Functionality']");
    }
);

casper.run(function() {this.test.renderResults(true);});
