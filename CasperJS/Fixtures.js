exports.create = function() {
	return new Fixtures();
};

var Fixtures = function Fixtures() { };

exports.Fixtures = Fixtures;

Fixtures.prototype.getPasswordArray = function getPasswordArray()
{
	var DATE = function DATE()
	{
		this.year = 2012;
		this.grantee = 102;
		this.section = 4;

		this.initDescription = "Automated test DATE";
		this.initFolder = '1';
		this.initCategory = '2';

		this.editDescription = "Edited Automated test DATE";
		this.editFolder = '5';
		this.editCategory = '';

		this.initFolderName = "New automated date folder";
		this.editFolderName = "Edited date folder";

		this.secondFolderName = "Second date folder";
		this.categoryName = "DATE category";
		this.editCategoryName = "edit DATE category";

	}

	return new DATE();
}

Fixtures.prototype.getFormula = Fixtures.prototype.getFormulaAlabama2012 = function getFormula()
{
	var Formula = function Formula()
	{
		this.year = 2012;
		this.grantee = 2;
		this.section = 5;

		this.initDescription = "Automated test Formula";
		this.initFolder = '12';
		this.initCategory = '';

		this.editDescription = "Edited Automated test Formula";
		this.editFolder = '90';
		this.editCategory = '';

		this.initFolderName = "New automated formula folder";
		this.editFolderName = "Edited formula folder";

		this.secondFolderName = "Second Formula folder";
		this.categoryName = "Formula category";
		this.editCategoryName = "edit Formula category";
	}

	return new Formula();
}

Fixtures.prototype.getDiscretionary = function getFormula()
{
	var Discretionary = function Discretionary()
	{
		this.year = 2012;
		this.grantee = 79;
		this.section = 6;

		this.initDescription = "Automated test Discretionary";
		this.initFolder = '14';
		this.initCategory = '';

		this.editDescription = "Edited Automated test Discretionary";
		this.editFolder = '91';
		this.editCategory = '';

		this.initFolderName = "New automated discretionary folder";
		this.editFolderName = "Edited discretionary folder";

		this.secondFolderName = "Second Discretionary folder";
		this.categoryName = "Discretionary category";
		this.editCategoryName = "edit Discretionary category";
	}

	return new Discretionary();
}

Fixtures.prototype.get = function getGenericFormula(year, grantee)
{
	var genFile = this.getFormula();
	genFile.year = year;
	genFile.grantee = grantee;

}
