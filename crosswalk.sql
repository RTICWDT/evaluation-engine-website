-- Table: "Crosswalk"

-- DROP TABLE "Crosswalk";

CREATE TABLE "Crosswalk"
(
  "HashedId" text NOT NULL,
  "CipherText" text NOT NULL,
  "Vector" text NOT NULL,
  "StudyId" text NOT NULL,
  CONSTRAINT "CrosswalkPrimaryKey" PRIMARY KEY ("HashedId")
)
WITH (
  OIDS=FALSE
);
ALTER TABLE "Crosswalk"
  OWNER TO eeconsoleuser;
