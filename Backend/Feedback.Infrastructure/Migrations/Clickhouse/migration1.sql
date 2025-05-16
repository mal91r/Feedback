CREATE TABLE IF NOT EXISTS FeedbackMessages
(
    ClientId  UUID,
    Message text,
    Tone Int32
)
    engine = MergeTree
    PARTITION BY Tone
    ORDER BY ClientId;