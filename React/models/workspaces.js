const mongoose = require('mongoose');

const workspaceSchema = new mongoose.Schema({
  jobPostId: {
    type: String,
  },
  userId: {
    type: String,
  },
});

module.exports = mongoose.model('workspaces', workspaceSchema);
