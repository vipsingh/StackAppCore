drop table if exists usermaster CASCADE;
drop table if exists userrole CASCADE;
drop table if exists useraddress CASCADE;
drop table if exists usersetting CASCADE;

CREATE TABLE usermaster
(
    id integer NOT NULL,
    name character varying(100) COLLATE pg_catalog."default" NOT NULL,
    loginid character varying(50),
    password character varying(50),
    role integer,
    submitamount numeric(20, 5),
    assigndate date,
    type integer,
    createdon timestamp without time zone,
    updatedon timestamp without time zone,
    CONSTRAINT usermaster_pkey PRIMARY KEY (id),
    CONSTRAINT usermaster_loginid_key UNIQUE (loginid)
)

CREATE TABLE userrole
(
    id integer NOT NULL,
    name character varying(100) COLLATE pg_catalog."default" NOT NULL,    
    info character varying(100),
    category integer,
    isadmin BOOLEAN,
    createdon timestamp without time zone,
    updatedon timestamp without time zone,
    CONSTRAINT userrole_pkey PRIMARY KEY (id)
)

CREATE TABLE useraddress
(
    id integer NOT NULL,
    name character varying(100) COLLATE pg_catalog."default" NOT NULL,
    type integer,
    city character varying(100),
    country integer,
    mobile character varying(15),
    usermaster_addresses__id integer,
    createdon timestamp without time zone,
    updatedon timestamp without time zone,
    CONSTRAINT useraddress_pkey PRIMARY KEY (id)
)

CREATE TABLE usersetting
(
    id integer NOT NULL,    
    currency integer,
    timezone character varying(10),
    isprimary boolean,
    usermaster_setting__id integer,
    createdon timestamp without time zone,
    updatedon timestamp without time zone,
    CONSTRAINT usersetting_pkey PRIMARY KEY (id)
)