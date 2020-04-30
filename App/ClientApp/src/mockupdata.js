window._AppSetting = {
    UserId: 1,
    RoleId: 1,
    DateFormat: "DD-MM-YYYY",
    BaseUrl: "/",
    ApiUrl: "@ViewBag.Host",
    BaseCurrency: 1,
    BaseCurrencySymbol: "INR",
    DecimalPlaces: 2,
    Language: "en"
  };
  
export const entity_detail = {
        "PageType": 2,
        "Widgets": {
            "Name": {
                "WidgetType": 1,
                "Caption": "Name",
                "WidgetId": "Name",
                "IsHidden": false,
                "Value": "user 2",
                "AdditionalValue": null,
                "FormatedValue": "user 2",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": {
                    "REQUIRED": {
                        "Msg": "Name is required"
                    }
                },
                "IsReadOnly": false,
                "IsRequired": true,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "LoginId": {
                "WidgetType": 1,
                "Caption": "LoginId",
                "WidgetId": "LoginId",
                "IsHidden": false,
                "Value": "user@1",
                "AdditionalValue": null,
                "FormatedValue": "user@1",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "AssignDate": {
                "WidgetType": 2,
                "Caption": "AssignDate",
                "WidgetId": "AssignDate",
                "IsHidden": false,
                "Value": "07-Apr-2020",
                "AdditionalValue": null,
                "FormatedValue": "07-Apr-2020",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "Role": {
                "WidgetType": 7,
                "Caption": "Role",
                "WidgetId": "Role",
                "IsHidden": false,
                "Value": {
                    "Value": 1,
                    "Text": "role 1"
                },
                "AdditionalValue": {
                    "ViewLink": {
                        "ActionId": "DETAIL",
                        "Title": "DETAIL",
                        "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp+LQhbUyb8KGPRU5oA4onNXtIaIKVF7XE7vRsCjvHbGor8/TgbjkohSTjBZUDbljgg==",
                        "ActionType": 0,
                        "ActionPosition": 0,
                        "DisplayType": 0,
                        "Icon": null,
                        "ExecutionType": 4,
                        "Attributes": {}
                    }
                },
                "FormatedValue": "role 1",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "Type": {
                "WidgetType": 6,
                "Options": null,
                "Caption": "Type",
                "WidgetId": "Type",
                "IsHidden": false,
                "Value": {
                    "Value": 2,
                    "Text": "Type Y"
                },
                "AdditionalValue": null,
                "FormatedValue": "Type Y",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": [
                    0,
                    1
                ],
                "IsEditable": false
            },
            "SubmitAmount": {
                "WidgetType": 3,
                "DecimalPlaces": 0,
                "Caption": "SubmitAmount",
                "WidgetId": "SubmitAmount",
                "IsHidden": false,
                "Value": "1000.00",
                "AdditionalValue": null,
                "FormatedValue": "1000.00",
                "WidgetFormatInfo": null,
                "IsViewMode": true,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            }
        },
        "FormRules": [
            {
                "Id": 0,
                "Type": "hidden",
                "Criteria": {
                    "0": [
                        "Type",
                        "0",
                        "2"
                    ]
                },
                "Fields": [
                    "SubmitAmount"
                ]
            },
            {
                "Id": 1,
                "Type": "readonly",
                "Criteria": {
                    "0": [
                        "Type",
                        "0",
                        "2"
                    ]
                },
                "Fields": [
                    "Role"
                ]
            }
        ],
        "PostUrl": "/entity/save?q=n5FqOPI/B14+HSUuwJVMp63O/lPiElD82fwpeHSEA6TKNtexZ25xqYgT3i10MJiTOnWL+NPeQbHTs/8cRdDItA==",
        "Actions": {
            "ACT_1": {
                "ActionId": "ACT_1",
                "Title": "View 1",
                "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp4vFBdZDoGdEjCpHxkD6TMl5LN0ew4h34A2EhvycSrYzFOs8/8Iun6we3VycRNhDsnMpVjWurZi8agR1PdniM5E=",
                "ActionType": 3,
                "ActionPosition": 0,
                "DisplayType": 0,
                "Icon": null,
                "ExecutionType": 4,
                "Attributes": {}
            },
            "FUN_1": {
                "ActionId": "FUN_1",
                "Title": "FUN 1",
                "Url": "/entity/execfunction?q=n5FqOPI/B14+HSUuwJVMp63O/lPiElD82fwpeHSEA6TKNtexZ25xqYgT3i10MJiTOnWL+NPeQbHTs/8cRdDItA==",
                "ActionType": 23,
                "ActionPosition": 0,
                "DisplayType": 0,
                "Icon": null,
                "ExecutionType": 6,
                "Attributes": {
                    "Function": null
                }
            },
            "BTN_EDIT": {
                "ActionId": "BTN_EDIT",
                "Title": null,
                "Url": "/entity/edit?q=n5FqOPI/B14+HSUuwJVMp63O/lPiElD82fwpeHSEA6TKNtexZ25xqYgT3i10MJiTOnWL+NPeQbHTs/8cRdDItA==",
                "ActionType": 2,
                "ActionPosition": 0,
                "DisplayType": 0,
                "Icon": null,
                "ExecutionType": 4,
                "Attributes": {}
            }
        },
        "EntityInfo": {
            "ObjectId": 3,
            "EntityId": 1
        },
        "ErrorMessage": null,
        "Layout": {
            "Header": {
                "Text": null,
                "Groups": [
                    {
                        "Text": null,
                        "Style": null,
                        "Rows": [
                            {
                                "Fields": [
                                    {
                                        "FieldId": "Name",
                                        "Text": null,
                                        "FullRow": false,
                                        "InVisible": null
                                    },
                                    {
                                        "FieldId": "LoginId",
                                        "Text": null,
                                        "FullRow": false,
                                        "InVisible": null
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            "RenderingStyle": 0,
            "Pages": [
                {
                    "Text": "general",
                    "Groups": [
                        {
                            "Text": "section 1",
                            "Style": null,
                            "Rows": [
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "AssignDate",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        },
                                        {
                                            "FieldId": "Role",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                },
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "Type",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        },
                                        {
                                            "FieldId": "RoleInfo",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                },
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "SubmitAmount",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            "Commands": [
                {
                    "Id": "ACT_1",
                    "Text": "View Role",
                    "Position": null
                },
                {
                    "Id": "FUN_1",
                    "Text": "Call FUN",
                    "Position": null
                }
            ]
        }
    };

export const entity_edit = {
        "PageType": 1,
        "Widgets": {
            "Name": {
                "WidgetType": 1,
                "Caption": "Name",
                "WidgetId": "Name",
                "IsHidden": false,
                "Value": "user 2",
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": {
                    "REQUIRED": {
                        "Msg": "Name is required"
                    }
                },
                "IsReadOnly": false,
                "IsRequired": true,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "LoginId": {
                "WidgetType": 1,
                "Caption": "LoginId",
                "WidgetId": "LoginId",
                "IsHidden": false,
                "Value": "user@1",
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": true,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "AssignDate": {
                "WidgetType": 2,
                "Caption": "AssignDate",
                "WidgetId": "AssignDate",
                "IsHidden": false,
                "Value": "2020-04-07T00:00:00",
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false
            },
            "Role": {
                "WidgetType": 7,
                "Caption": "Role",
                "WidgetId": "Role",
                "IsHidden": false,
                "Value": {
                    "Value": 1,
                    "Text": "role 1"
                },
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "IsMultiSelect": true,
                "DataActionLink": {
                    "ActionId": null,
                    "Title": null,
                    "Url": "widget/GetPickerData?q=Tl6QQLwHB9RKTO2s0kyR6zXmA85ZdUZo2fK1k95bw9/faLEfL/NcJjqEpo1Z+AR3w8RCYl91ZSecZ/E11CTcUIkpXuuTnt7cPJG/S0mok5E=",
                    "ActionType": 50,
                    "ActionPosition": 0,
                    "DisplayType": 0,
                    "Icon": null,
                    "ExecutionType": 6,
                    "Attributes": {}
                },
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false,
                "Dependency": {
                    "Parents": [
                        {
                            "WidgetId": "Type",
                            "LinkType": 1,
                            "IsRequired": false,
                            "Parameters": null

                        }
                    ]
                }
            },
            "Type": {
                "WidgetType": 6,
                "Options": [
                    {
                        "Value": 1,
                        "Text": "Type X"
                    },
                    {
                        "Value": 2,
                        "Text": "Type Y"
                    },
                    {
                        "Value": 3,
                        "Text": "Type Z"
                    },
                    {
                        "Value": 4,
                        "Text": "Type U"
                    }
                ],
                "Caption": "Type",
                "WidgetId": "Type",
                "IsHidden": false,
                "Value": {
                    "Value": 2,
                    "Text": "Type Y"
                },
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": [
                    0,
                    1
                ],
                "IsEditable": false,
                "Dependency": {
                    "Children": [
                        {
                            "WidgetId": "Role",
                            "Clear": true
                        }
                    ]
                }
            },
            "SubmitAmount": {
                "WidgetType": 3,
                "DecimalPlaces": 0,
                "Caption": "SubmitAmount",
                "WidgetId": "SubmitAmount",
                "IsHidden": false,
                "Value": 1000.00000,
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {IsMonatryAmount: true},
                "Validation": { "RANGE": { "Min": 0, "Max": 20000, "ValueType": "DECIMAL"} },
                "IsReadOnly": false,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "IsEditable": false,
                "RuleToFire": [
                    2
                ],
            },
            "ComputedAmount": {
                "WidgetType": 3,
                "DecimalPlaces": 0,
                "Caption": "ComputedAmount",
                "WidgetId": "ComputedAmount",
                "IsHidden": false,
                "AdditionalValue": null,
                "FormatedValue": null,
                "WidgetFormatInfo": null,
                "IsViewMode": false,
                "Properties": {},
                "Validation": null,
                "IsReadOnly": true,
                "IsRequired": false,
                "DataActionLink": null,
                "ActionLink": null,
                "RuleToFire": null,
                "IsEditable": false                
            }
        },
        "FormRules": [
            {
                "Id": 0,
                "Type": "hidden",
                "Criteria": {
                    "0": [
                        "Type",
                        "0",
                        "2"
                    ]
                },
                "Fields": [
                    "SubmitAmount"
                ]
            },
            {
                "Id": 1,
                "Type": "readonly",
                "Criteria": {
                    "0": [
                        "Type",
                        "0",
                        "2"
                    ]
                },
                "Fields": [
                    "Role"
                ]
            },
            { "Index": 2, "Type": "EVAL", "Exp": "SubmitAmount + 100", "Field": "ComputedAmount", "ExpFields": ["SubmitAmount"] }
        ],
        "PostUrl": "/entity/save?q=n5FqOPI/B14+HSUuwJVMp63O/lPiElD82fwpeHSEA6TKNtexZ25xqYgT3i10MJiTOnWL+NPeQbHTs/8cRdDItA==",
        "Actions": {
            "BTN_SAVE": {
                "ActionId": "BTN_SAVE",
                "Title": null,
                "Url": "entity/save?q=n5FqOPI/B14+HSUuwJVMp63O/lPiElD82fwpeHSEA6TKNtexZ25xqYgT3i10MJiTOnWL+NPeQbHTs/8cRdDItA==",
                "ActionType": 10,
                "ActionPosition": 0,
                "DisplayType": 0,
                "Icon": null,
                "ExecutionType": 1,
                "Attributes": {}
            }
        },
        "EntityInfo": {
            "ObjectId": 3,
            "EntityId": 1
        },
        "ErrorMessage": null,
        "Layout": {
            "Header": {
                "Text": null,
                "Groups": [
                    {
                        "Text": null,
                        "Style": null,
                        "Rows": [
                            {
                                "Fields": [
                                    {
                                        "FieldId": "Name",
                                        "Text": null,
                                        "FullRow": false,
                                        "InVisible": null
                                    },
                                    {
                                        "FieldId": "LoginId",
                                        "Text": null,
                                        "FullRow": false,
                                        "InVisible": null
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            "RenderingStyle": 0,
            "Pages": [
                {
                    "Text": "general",
                    "Groups": [
                        {
                            "Text": "section 1",
                            "Style": null,
                            "Rows": [
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "AssignDate",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        },
                                        {
                                            "FieldId": "Role",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                },
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "Type",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        },
                                        {
                                            "FieldId": "RoleInfo",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                },
                                {
                                    "Fields": [
                                        {
                                            "FieldId": "SubmitAmount",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        },
                                        {
                                            "FieldId": "ComputedAmount",
                                            "Text": null,
                                            "FullRow": false,
                                            "InVisible": null
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            "Commands": [
                {
                    "Id": "ACT_1",
                    "Text": "View Role",
                    "Position": null
                },
                {
                    "Id": "FUN_1",
                    "Text": "Call FUN",
                    "Position": null
                }
            ]
        }
    };

export const widget_data = {
    "Fields": {
        "ID": {
            "WidgetType": 1,
            "Caption": "id",
            "WidgetId": "id",
            "IsHidden": false,
            "Value": null,
            "AdditionalValue": null,
            "FormatedValue": null,
            "WidgetFormatInfo": null,
            "IsViewMode": true,
            "Properties": {},
            "Validation": null,
            "IsReadOnly": true,
            "IsRequired": false,
            "DataActionLink": null,
            "ActionLink": null,
            "RuleToFire": null,
            "IsEditable": false
        },
        "NAME": {
            "WidgetType": 1,
            "Caption": "Name",
            "WidgetId": "Name",
            "IsHidden": false,
            "Value": "role 1",
            "AdditionalValue": {
                "ViewLink": {
                    "ActionId": "DETAIL",
                    "Title": "DETAIL",
                    "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp+LQhbUyb8KGPRU5oA4onNXtIaIKVF7XE7vRsCjvHbGor8/TgbjkohSTjBZUDbljgg==",
                    "ActionType": 0,
                    "ActionPosition": 0,
                    "DisplayType": 0,
                    "Icon": null,
                    "ExecutionType": 4,
                    "Attributes": {}
                }
            },
            "FormatedValue": "role 1",
            "WidgetFormatInfo": null,
            "IsViewMode": true,
            "Properties": {},
            "Validation": {
                "REQUIRED": {
                    "Msg": "Name is required"
                }
            },
            "IsReadOnly": false,
            "IsRequired": true,
            "DataActionLink": null,
            "ActionLink": null,
            "RuleToFire": null,
            "IsEditable": false
        }
    },
    "Properties": null,
    "Data": [
        {
            "RowId": 2,
            "Name": {
                "FormatedValue": "role 2",
                "AdditionalValue": {
                    "ViewLink": {
                        "ActionId": "DETAIL",
                        "Title": "DETAIL",
                        "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp+LQhbUyb8KGPRU5oA4onNXtIaIKVF7XE7vRsCjvHbGoLsvn+D/eOe55TzfIoRWnag==",
                        "ActionType": 0,
                        "ActionPosition": 0,
                        "DisplayType": 0,
                        "Icon": null,
                        "ExecutionType": 4,
                        "Attributes": {},
                        "Target": "POPUP"
                    }
                }
            }
        },
        {
            "RowId": 5,
            "Name": {
                "FormatedValue": "role 3",
                "AdditionalValue": {
                    "ViewLink": {
                        "ActionId": "DETAIL",
                        "Title": "DETAIL",
                        "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp+LQhbUyb8KGPRU5oA4onNXtIaIKVF7XE7vRsCjvHbGoQ3Jk3ZYqrc975s/2Jrlz9Q==",
                        "ActionType": 0,
                        "ActionPosition": 0,
                        "DisplayType": 0,
                        "Icon": null,
                        "ExecutionType": 4,
                        "Attributes": {}
                    }
                }
            }
        },
        {
            "RowId": 1,
            "Name": {
                "FormatedValue": "role 1",
                "AdditionalValue": {
                    "ViewLink": {
                        "ActionId": "DETAIL",
                        "Title": "DETAIL",
                        "Url": "/entity/detail?q=n5FqOPI/B14+HSUuwJVMp+LQhbUyb8KGPRU5oA4onNXtIaIKVF7XE7vRsCjvHbGor8/TgbjkohSTjBZUDbljgg==",
                        "ActionType": 0,
                        "ActionPosition": 0,
                        "DisplayType": 0,
                        "Icon": null,
                        "ExecutionType": 4,
                        "Attributes": {}
                    }
                }
            }
        }
    ],
    "RenderMode": null,
    "IdColumn": "Id",
    "ItemViewField": "Name",
    "Layout": null,
    "ActionButtons": null,
    "PageSize": 0,
    "PageIndex": 0
};