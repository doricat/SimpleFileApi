create sequence id_seq;

create table if not exists file_metadata
(
  id           bigint                              not null
    constraint file_metadata_pk
      primary key,
  file_name    varchar(250)                        not null,
  content_type varchar(50)                         not null,
  size         integer                             not null,
  created_at   timestamp default CURRENT_TIMESTAMP not null
);