truncate table autoid;

CREATE TABLE autoid
(
    entityname character varying(100) COLLATE pg_catalog."default" NOT NULL,
    maxid integer NOT NULL,
    CONSTRAINT autoid_pkey PRIMARY KEY (entityname)
)

CREATE TABLE entitymaster
(
	id integer GENERATED ALWAYS AS IDENTITY,
	name character varying(100)  NOT NULL,
	text character varying(200) ,
	tablename character varying(60) ,
	primaryfield character varying(100) ,
	namefield character varying(60) ,
	CONSTRAINT entitymaster_pkey PRIMARY KEY (id),
	CONSTRAINT entitymaster_name_key UNIQUE (name)
)

CREATE TABLE entityschema
(
	id integer GENERATED ALWAYS AS IDENTITY,
	entityid integer NOT NULL,
	fieldname character varying(100)  NOT NULL,
	label character varying(100)  NOT NULL,
	fieldtype integer NOT NULL,
	length integer NOT NULL,
	viewtype integer default(0),
	linkentity integer,
	linkentity_domain character varying(500),	
	isrequired boolean,
	isreadonly boolean,
	defaultvalue character varying(200),
	dbname character varying(100),
	tablename character varying(100),
	uisetting character varying(500),
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT entitymaster_pkey PRIMARY KEY (id)
)

CREATE TABLE entity_itemtype
(
	id integer GENERATED ALWAYS AS IDENTITY,
	entityid integer NOT NULL, 
	name character varying(100)  NOT NULL,
	code character varying(5),
	text character varying(200),
	createdby__id integer,
	createdon timestamp without time zone,
	updatedby__id integer,
    updatedon timestamp without time zone,
	CONSTRAINT entity_itemtype_pkey PRIMARY KEY (id),
)

CREATE TABLE entity_viewlayout
(
	id integer GENERATED ALWAYS AS IDENTITY,
	entityid integer NOT NULL,
	itemtype integer NOT NULL,
	states character varying(100)  NOT NULL,	
	viewtype integer default(0),
	layoutxml xml,
	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT entity_viewlayout_pkey PRIMARY KEY (id),
)

CREATE TABLE company_master
(
	id integer GENERATED ALWAYS AS IDENTITY,
	name character varying(100)  NOT NULL,
	description character varying(200),	

	createdon timestamp without time zone,	
    updatedon timestamp without time zone,
	CONSTRAINT company_master_pkey PRIMARY KEY (id),
)