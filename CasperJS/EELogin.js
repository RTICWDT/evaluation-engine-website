exports.create = function(casper, userEmail, userPwd) {
	return new Login(casper, userEmail, userPwd);
};

var Login = function Login(casper, userEmail, userPwd)
{
	casper.waitForSelector("form input[name='UserName']",
	    function success() {
		this.test.assertExists("form input[name='UserName']");
		this.click("form input[name='UserName']");
	    },
	    function fail() {
		this.test.assertExists("form input[name='UserName']");
	});
	casper.waitForSelector("form",
	    function success() {
		this.fill("form", {"UserName": userEmail});
	    },
	    function fail() {
		this.test.assertExists("form");
	});
	casper.waitForSelector("form input[name='Password']",
	    function success() {
		this.test.assertExists("form input[name='Password']");
		this.click("form input[name='Password']");
	    },
	    function fail() {
		this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
	    function success() {
		this.fill("form", {"Password": userPwd});
	    },
	    function fail() {
		this.test.assertExists("form");
	});
	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.test.assertExists("form input[type=submit][value='Log in']");
			this.click("form input[type=submit][value='Log in']");
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});
	// submit form
}

exports.Login = Login;
