# WMIClass2Json

Tool for convert a WMI Class definition to Json.

## Use
```
WMIClass2Json.exe WMIClassName
```

## Example
Json definition for Win32_Registry:
```
> WMIClass2Json.exe Win32_Registry
{"name":"Win32_Registry","fields":{"Caption":"String","CurrentSize":"UInt32","Description":"String","InstallDate":"DateTime","MaximumSize":"UInt32","Name":"String","ProposedSize":"UInt32","Status":"String"}}
```

Beautified, (thanks [codebeautify](http://codebeautify.org/jsonviewer#)!):
```
{
    "name": "Win32_Registry",
    "fields": {
        "Caption": "String",
        "CurrentSize": "UInt32",
        "Description": "String",
        "InstallDate": "DateTime",
        "MaximumSize": "UInt32",
        "Name": "String",
        "ProposedSize": "UInt32",
        "Status": "String"
    }
}
```

## Why?

This is a tool of a bigger project (publicly soon, I hope), would be an useful example of:
* Retrieve information from WMI Classes.
* Serialize objects with .Net Framework.
