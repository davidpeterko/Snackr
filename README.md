# Snackr
Snackr project for learning experience.

# Inventory
Implemented with Apache Cassandra for storage and scalability.  
Create Table Code:  

```sql
  CREATE TABLE snackapi.snack_counts (
    snack_brand text,
    snack_name text,
    snack_count int,
    PRIMARY KEY (snack_brand, snack_name)
) WITH CLUSTERING ORDER BY (snack_name ASC)
    AND bloom_filter_fp_chance = 0.01
    AND caching = {'keys': 'ALL', 'rows_per_partition': 'NONE'}
    AND comment = ''
    AND compaction = {'class': 'org.apache.cassandra.db.compaction.SizeTieredCompactionStrategy', 'max_threshold': '32', 'min_threshold': '4'}
    AND compression = {'chunk_length_in_kb': '64', 'class': 'org.apache.cassandra.io.compress.LZ4Compressor'}
    AND crc_check_chance = 1.0
    AND dclocal_read_repair_chance = 0.1
    AND default_time_to_live = 0
    AND gc_grace_seconds = 864000
    AND max_index_interval = 2048
    AND memtable_flush_period_in_ms = 0
    AND min_index_interval = 128
    AND read_repair_chance = 0.0
    AND speculative_retry = '99PERCENTILE';
```

# Requests
Requests model implemented and stored in Apache Cassandra. 
Future additions: ability to select delivery option. 

# Authentication and User Management
Using auth0 to manage user logins.  
Implemented onyl allowing genebygene domain users to login. 