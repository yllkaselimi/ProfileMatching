const mongoose = require('mongoose');

const WorkspaceSchema = new mongoose.Schema({
  WorkspaceID: {
    type: String,
    required: true,
  },
  JobPostID: {
    type: String,
  },
  ClientID: {
    type: String,
  },
});

module.exports = mongoose.model('Workspace', WorkspaceSchema);
