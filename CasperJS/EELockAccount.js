exports.create = function(casper, userEmail) {
	return new LockAccount(casper, userEmail);
};

var LockAccount = function LockAccount(casper, userEmail)
{
	casper.waitForSelector("form input[name='UserName']",
		function success() {
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
			this.click("form input[name='Password']");
		},
		function fail() {
			this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
		function success() {
			this.fill("form", {"Password": badPassword});
		},
		function fail() {
			this.test.assertExists("form");
	});

	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.click("form input[type=submit][value='Log in']");
			this.capture(imagesPath + 'account_locked_fail.png');
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});
	
	casper.waitForSelector("form input[name='UserName']",
		function success() {
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
			this.click("form input[name='Password']");
		},
		function fail() {
			this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
		function success() {
			this.fill("form", {"Password": badPassword});
		},
		function fail() {
			this.test.assertExists("form");
	});

	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.click("form input[type=submit][value='Log in']");
			this.capture(imagesPath + 'account_locked_fail.png');
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});

		casper.waitForSelector("form input[name='UserName']",
		function success() {
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
			this.click("form input[name='Password']");
		},
		function fail() {
			this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
		function success() {
			this.fill("form", {"Password": badPassword});
		},
		function fail() {
			this.test.assertExists("form");
	});

	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.click("form input[type=submit][value='Log in']");
			this.capture(imagesPath + 'account_locked_fail.png');
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});
	
	casper.waitForSelector("form input[name='UserName']",
		function success() {
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
			this.click("form input[name='Password']");
		},
		function fail() {
			this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
		function success() {
			this.fill("form", {"Password": badPassword});
		},
		function fail() {
			this.test.assertExists("form");
	});

	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.click("form input[type=submit][value='Log in']");
			this.capture(imagesPath + 'account_locked_fail.png');
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});

		casper.waitForSelector("form input[name='UserName']",
		function success() {
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
			this.click("form input[name='Password']");
		},
		function fail() {
			this.test.assertExists("form input[name='Password']");
	});
	casper.waitForSelector("form",
		function success() {
			this.fill("form", {"Password": badPassword});
		},
		function fail() {
			this.test.assertExists("form");
	});

	casper.waitForSelector("form input[type=submit][value='Log in']",
		function success() {
			this.click("form input[type=submit][value='Log in']");
			this.capture(imagesPath + 'account_locked_fail.png');
		},
		function fail() {
			this.test.assertExists("form input[type=submit][value='Log in']");
	});
}

exports.LockAccount = LockAccount;