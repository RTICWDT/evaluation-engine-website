using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using EvalEngine.UI.Models;

public class CheckListAttribute : ValidationAttribute
{
    int _length;
    bool _fixed;
    string _errMessage;

    public override bool IsValid(object value)
    {
        int l = 0;

        if (value != null && typeof(List<int>) == value.GetType())
        {
            var v = (List<int>)value;
            l = v.Count;
        }
        else if (value != null)
        {
            var v = (List<CheckboxItem>)value;
            v = v.FindAll(x => x.Checked.Equals(true));
            l = v.Count;
        }

        if (this._fixed)
        {
            if (value != null && this._length == l)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (value != null && l >= this._length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        int l = 0;

        if (value != null && typeof(List<int>) == value.GetType())
        {
            var v = (List<int>)value;
            l = v.Count;
        }
        else if (value != null)
        {
            var v = (List<CheckboxItem>)value;
            v = v.FindAll(x => x.Checked.Equals(true));
            l = v.Count;
        }

        if (this._fixed)
        {
            if (value != null && this._length == l)
            {
                return null;
            }
            else
            {
                return new ValidationResult(_errMessage);
            }
        }
        else
        {
            if (value != null && l >= this._length)
            {
                return null;
            }
            else
            {
                return new ValidationResult(_errMessage);
            }
        }
    }

    public CheckListAttribute(int length = 1, bool isFixed = false, string errMessage = "There was an error with the data you entered")
    {

        this._length = length;
        this._fixed = isFixed;
        this._errMessage = errMessage;
    }

}