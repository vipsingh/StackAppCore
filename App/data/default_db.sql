--User ID=eiklvzyt;Password=tJ_N5qAB8jMMwtPYATj6CBaIPjNEGnkZ;Host=isilo.db.elephantsql.com;Port=5432;Database=eiklvzyt;Pooling=true;
--entity ids => 1, 101 , user>10000
--fields     => 1-10000, 10001-1000000, user>1000000+

--user defind id shoul start with 1000000 (on master tables which have system defined entries)
CREATE TABLE t_application (

)
truncate table autoid;

CREATE TABLE autoid
(
    entityid integer NOT NULL,
    maxid integer NOT NULL,
    CONSTRAINT autoid_pkey PRIMARY KEY (entityid)
);

CREATE TABLE t_app_modules 
(
	id integer,
	name character varying(100)  NOT NULL,
	text character varying(500) ,
	CONSTRAINT t_app_modules_pkey PRIMARY KEY (id),
	CONSTRAINT t_app_modules_name_key UNIQUE (name)
);

CREATE TABLE t_app_menu (
	masterid integer NOT NULL,
	appid integer NOT NULL,
	id integer,	
	menujson json NOT NULL,
	CONSTRAINT t_app_menu_pkey PRIMARY KEY (id)
);

CREATE TABLE t_entitymaster
(
	masterid integer NOT NULL,
	id integer,
	name character varying(100)  NOT NULL,
	text character varying(200) ,
	tablename character varying(60) ,
	appmodule integer NOT NULL default(1),
	primaryfield character varying(100) ,
	namefield character varying(60) ,
	createdby integer NOT NULL,
    updatedby integer,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_entitymaster_pkey PRIMARY KEY (id),
	CONSTRAINT t_entitymaster_name_key UNIQUE (name)
);

CREATE TABLE t_entityschema
(
	masterid integer NOT NULL,
	id integer,
	entityid integer NOT NULL,
	fieldname character varying(100)  NOT NULL,
	label character varying(100)  NOT NULL,
	fieldtype integer NOT NULL,
	length integer NOT NULL,
	viewtype integer default(0),
	collectionid integer,
	-- relationshipid integer,
	linkentity integer,
	linkentity_domain character varying(500),	
	displayexp character varying(500),
	linkentity_field char varying(60),
	ismultiselect boolean,
	relatedexp character varying(200),
	isunique boolean,
	isrequired boolean,
	isreadonly boolean,
	defaultvalue character varying(200),
	dbname character varying(100),
	tablename character varying(100),
	computeexpression character varying(1024),
	uisetting character varying(500),
	vieworder int,
	createdby integer NOT NULL,
    updatedby integer,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_entityschema_pkey PRIMARY KEY (id),
	CONSTRAINT t_entityschema_fieldname_key UNIQUE (entityid,fieldname)
);

CREATE TABLE t_entity_relationship
(
	entityid integer NOT NULL,
	related_entityid integer NOT NULL,
	type integer NOT NULL,
	targetfield char varying(60),	
	CONSTRAINT t_entity_relationship_pkey PRIMARY KEY (entityid,related_entityid,targetfield)
);

CREATE TABLE t_entity_itemtype
(
	masterid integer NOT NULL,
	id integer,
	entityid integer NOT NULL, 
	name character varying(100)  NOT NULL,
	code character varying(5),
	text character varying(200),
	createdby integer,
	createdon timestamp without time zone,
	updatedby integer,
    updatedon timestamp without time zone,
	CONSTRAINT t_entity_itemtype_pkey PRIMARY KEY (id),
	CONSTRAINT t_entity_itemtype_name_key UNIQUE (entityid,name)
);

CREATE TABLE t_entity_viewlayout
(
	masterid integer NOT NULL,
	id integer,
	entityid integer NOT NULL,
	itemtype integer NOT NULL,
	states character varying(100)  NOT NULL,	
	viewtype integer default(0),
	layoutxml xml,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_entity_viewlayout_pkey PRIMARY KEY (id)
);

create table t_collection_master(
	masterid integer NOT NULL,
	id integer not null,
	dataid integer not null,
	datatext character varying(100) ,
	datainfo character varying(500) ,
	datacode character varying(60) ,
	fieldid integer,
	groupname character varying(100) ,
	CONSTRAINT t_collection_master_pkey PRIMARY KEY (id,dataid)
);


create table t_entityactions
(
	masterid integer NOT NULL,
	id integer,
	entityid integer NOT NULL,
	viewtype integer NOT NULL,
	text character varying(200),
	actiontype integer NOT NULL,
	visibility character varying(1024),
	action character varying(200),
	queryparam character varying(1024),
	dataparam character varying(1024),
	confirmmessage character varying(200),
	operations character varying(1024),
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_entityactions_pkey PRIMARY KEY (id)
);

create table t_entitylist
(
	masterid integer NOT NULL,
	id integer,
	entityid integer NOT NULL,
	categoryid integer NOT NULL,
	name character varying(60) NOT NULL,
	description character varying(200),
	idfield character varying(60)  NOT NULL,
	viewfield character varying(60),
	orderby character varying(200),
	layoutxml text,
	fixedfilter character varying(1000),
	filterpolicy character varying(1000),
	additional character varying(1000),
	sqlquery text,
	recordlimit integer,
	createdby integer NOT NULL,
    updatedby integer,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_entitylist_pkey PRIMARY KEY (id)
);

insert into autoid values(1, 10000);
insert into autoid values(2, 1000000);

insert into t_app_modules values(1, 'Other', 'Others');
insert into t_app_modules values(2, 'Core', 'Core');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,1,1,'Default','0');
insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,1,1,1,'','0', null);

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,2,2,'Default','0');
insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,2,2,2,'','0', null);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,1,1,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="text"/><field id="tablename"/><field id="primaryfield"/><field id="namefield"/></row></template></tlist>',
'','', '2020-01-01',1);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,2,2,0,'Default','id','fieldname',
'<tlist type="GRID"><template><row><field id="fieldname"/><field id="entityid"/><field id="fieldtype"/><field id="collectionid"/><field id="linkentity"/></row></template></tlist>',
'','', '2020-01-01',1);
-----------------------------------------------------------------------------------------------

CREATE TABLE t_company_master
(	
	id integer,
	masterid integer NOT NULL,
	name character varying(100)  NOT NULL,
	description character varying(200),	
	createdby integer NOT NULL,
    updatedby integer,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_company_master_pkey PRIMARY KEY (id)
)
insert into t_company_master values(1,10,'Default Company', '',1, null, '2020-01-01', null);

-----------------------------------------------

CREATE TABLE t_user
(	
    masterid integer NOT NULL,
	id integer,	
	name character varying(100) NOT NULL,
	shortname character varying(200),	
    loginid character varying(100) not null,
    logintype integer not null,
    password character varying(1000) not null,
    type integer not null,
    mobileno character varying(15),
    emailid character varying(100),
    isactive boolean default(true),
    expiredon date,	    
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT t_user_pkey PRIMARY KEY (id),
    CONSTRAINT t_user_loginid_key UNIQUE (loginid),
    CONSTRAINT t_user_mobileno_key UNIQUE (mobileno),
    CONSTRAINT t_user_emailid_key UNIQUE (emailid)
);

CREATE TABLE t_company_setting
(
    masterid integer NOT NULL,
	companyid integer,	
    timezone character varying(30) NOT NULL,
    language character varying(20) NOT NULL,
    defaultcurrency integer NOT NULL,
    financialyearmonth integer,
    shortdateformat character varying(30) default('dd-MMM-yyyy'),
    longdateformat character varying(40),
    CONSTRAINT t_company_setting_pkey PRIMARY KEY (companyid)
);

CREATE TABLE t_currency 
(
    masterid integer NOT NULL,
    id integer,
    code  character varying(30) NOT NULL,
    name character varying(60) NOT NULL,
    decimalplace integer not null,    
    CONSTRAINT t_currency_pkey PRIMARY KEY (id),
    CONSTRAINT t_currency_code_key UNIQUE (code)
);

CREATE TABLE t_role
(
    masterid integer NOT NULL,
    id integer,
    name character varying(60) NOT NULL,
    code character varying(10),
    profileid integer, 
    CONSTRAINT t_role_pkey PRIMARY KEY (id)
);

CREATE TABLE t_userrole
(
    masterid integer NOT NULL,
    userid integer not null,
    roleid integer not null,
    scope integer,
    CONSTRAINT t_userrole_pkey PRIMARY KEY (userid,roleid)
);

CREATE TABLE t_operation
(
    masterid integer NOT NULL,
    id integer,
    name character varying(60) NOT NULL,
    text character varying(200),
    entityid integer not null,
    operaiontype integer not null,
    CONSTRAINT t_operation_pkey PRIMARY KEY (id)
);

CREATE TABLE t_role_operation
(
    masterid integer NOT NULL,
    roleid integer not null,
    operation integer not null,    
    CONSTRAINT t_user_operation_pkey PRIMARY KEY (roleid,operation)
);
------------------------------------DATA----------------
insert into t_collection_master values(0,1,1,'Male',null,'M',null,null);
insert into t_collection_master values(0,1,2,'Female',null,'F',null,null);

insert into t_user values(10,1,'Vipin', null, 'vip@stackerp.com',2, '',1,'9899013097',null,true,null,'2020-01-01', null);
insert into t_role values(10,1,'System Admin', 'SA', 1);
insert into t_userrole values(10,1,1,1);
