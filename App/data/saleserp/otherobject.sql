CREATE TABLE t_customer
(
    masterid integer NOT NULL,
    id integer,	
	name character varying(100)  NOT NULL,
    type integer,
    userid integer not null,
    isactive boolean default(true),
    status integer not null,
    mobileno character varying(20),
    emailid character varying(100),
    addressline1 character varying(100) not null,
    addressline2 character varying(60),
    city integer NOT NULL,    
    state integer not null,
    area integer not null, --society
    gender integer,
    itemtype int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_customer_pkey PRIMARY KEY (id)
);

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,111,'Customer','Customer','t_customer','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10002,111,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10002,111,10002,'','0', null);

insert into autoid values(111, 0);

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10001,111,'name','Name',1,100,true
,'name','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10002,111,'type','Type',2,4,false
,'type','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10003,111,'userid','User',2,4,false
,'userid','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10004,111,'isactive','Active',7,4,false
,'isactive','t_customer',0,'1'
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue, collectionid
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10005,111,'status','Status',9,4,true
,'status','t_customer',0,null,10001
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10006,111,'mobileno','Mobile No',1,20,false
,'mobileno','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10007,111,'emailid','Email',1,100,false
,'emailid','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10008,111,'addressline1','Address Line 1',1,100,true
,'addressline1','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10009,111,'addressline2','Address Line 2',1,100,false
,'addressline2','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10010,111,'city','City',2,4,true
,'city','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10011,111,'state','State',2,4,true
,'state','t_customer',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10012,111,'area','Area',10,0,true
,'area','t_customer',0,null
,131,'[{"city": [0, "@city"]}]','2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,collectionid
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10013,111,'gender','Gender',9,0,false
,'gender','t_customer',0,null,1
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10001,111,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="status" /></row></template></tlist>',
'','', '2020-01-01',1);
-------------------------------------------------------------------------------------

CREATE TABLE t_city_area
(
    masterid integer NOT NULL,
    id integer,	
    name character varying(100) NOT NULL,
    city integer NOT NULL,
    pincode character varying(10) NOT NULL,
    itemtype int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_city_area_pkey PRIMARY KEY (id)
);

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10014,131,'name','Name',1,100,true
,'name','t_city_area',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired,isreadonly
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10015,131,'city','city',2,4,false,true
,'city','t_city_area',0,null
,130,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10016,131,'pincode','Pincode',1,10,true
,'pincode','t_city_area',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,131,'Area','Area','t_city_area','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10003,131,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10003,131,10003,'','0', null);

insert into autoid values(131, 0);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10002,131,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="city" /><field id="pincode" /></row></template></tlist>',
'','', '2020-01-01',1);
---------------------------------------------------

CREATE TABLE t_product_category
(
    masterid integer NOT NULL,
    id integer,	
	name character varying(100)  NOT NULL,
    groupname character varying(100),
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_product_category_pkey PRIMARY KEY (id)
);

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10017,112,'name','Name',1,100,true
,'name','t_product_category',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10018,112,'groupname','Group Name',1,100,false
,'groupname','t_product_category',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,112,'ProductCategory','Product Category','t_product_category','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10004,112,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10004,112,10004,'','0', null);

insert into autoid values(112, 0);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10003,112,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="groupname" /></row></template></tlist>',
'','', '2020-01-01',1);

--------------------------------------------------------------------------------
CREATE TABLE t_product_uom
(
    masterid integer NOT NULL,
    id integer,	
	name character varying(100)  NOT NULL,
    code character varying(20),
    groupid integer not null,
    itemtype int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_product_uom_pkey PRIMARY KEY (id)
);

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10019,113,'name','Name',1,100,true
,'name','t_product_uom',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10020,113,'code','Code',1,20,false
,'code','t_product_uom',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue,collectionid
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10021,113,'groupid','Group',9,0,true
,'groupid','t_product_uom',0,null,20
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,113,'ProductUom','Product UOM','t_product_uom','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10005,113,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10005,113,10005,'','0', null);

insert into autoid values(113, 0);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10004,113,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="groupid" /></row></template></tlist>',
'','', '2020-01-01',1);

--------------------------------------------------------------------------------------
CREATE TABLE t_product
(
    masterid integer NOT NULL,
    id integer,	
	name character varying(100) NOT NULL,
    shortname character varying(60),
    code character varying(40)  NOT NULL,
    category integer NOT NULL,
    type integer,
    catelogno character varying(40),
    uom integer not null,
    description character varying(1000),
    subdescription character varying(2000),
    tag1 character varying(100),
    tag2 character varying(100),
    image character varying(1000),
    orignalprice numeric(20,4) not null,
    itemtype int,
    createdby integer NOT NULL,
    updatedby integer,
    createdon timestamp without time zone NOT NULL,	
    updatedon timestamp without time zone,
    CONSTRAINT t_product_pkey PRIMARY KEY (id)
);
insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10022,115,'name','Name',1,100,true
,'name','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10023,115,'shortname','shortname',1,60,false
,'shortname','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10024,115,'code','Code',1,40,true
,'code','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10025,115,'category','Category',10,0,true
,'category','t_product',0,null
,112,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10026,115,'type','Type',2,4,false
,'type','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10027,115,'catelogno','CatelogNo',1,40,false
,'catelogno','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10028,115,'uom','Uom',10,0,true
,'uom','t_product',0,null
,113,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10029,115,'description','Description',13,1000,false
,'description','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10030,115,'subdescription','Sub Description',17,2000,false
,'subdescription','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10031,115,'tag1','Tag1',1,100,false
,'tag1','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10032,115,'tag2','Tag2',1,100,false
,'tag2','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10033,115,'orignalprice','Orignal Price',6,100,true
,'orignalprice','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');

insert into t_entityschema(masterid,id, entityid,fieldname,label,fieldtype,length,isrequired
,dbname,tablename,viewtype,defaultvalue
,linkentity,linkentity_domain,createdon,updatedon)
values(0,10034,115,'image','Image',14,0,false
,'image','t_product',0,null
,null,null,'2019-11-28 22:49:44','2019-11-28 22:49:44');
----
insert into t_entitymaster(masterid,id,name,text,tablename,primaryfield,namefield)
values(0,115,'Product','Product','t_product','id', 'name');

insert into t_entity_itemtype(masterid,id,entityid,name,code)
values(0,10006,115,'Default','0');

insert into t_entity_viewlayout(masterid,id,entityid,itemtype,states,viewtype,layoutxml)
values(0,10006,115,10006,'','0', null);

insert into autoid values(115, 0);

insert into t_entitylist(masterid,id,entityid,categoryid,name,idfield,viewfield,
	layoutxml, 
	filterpolicy,fixedfilter,createdon,createdby) 
values(0,10005,115,0,'Default','id','name',
'<tlist type="GRID"><template><row><field id="name"/><field id="category" /><field id="code" /><field id="uom" /><field id="orignalprice" /></row></template></tlist>',
'','', '2020-01-01',1);
----------------------------------------------------------------
---// Collection(10001) customer status
---// Collection(1) gender
---//Entity(131) t_city_area
---Entity(CITY) 130
---Collection(20) UOM group
