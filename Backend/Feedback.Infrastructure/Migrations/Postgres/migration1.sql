create table if not exists "FeedbackOwners" (
                                                "ClientId" uuid,
                                                "ChannelId" bigint primary key
);

CREATE UNIQUE INDEX idx_unique_feedback_owners ON "FeedbackOwners" ("ClientId","ChannelId");