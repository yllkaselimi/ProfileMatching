const mongoose = require('mongoose');

const cardSchema = new mongoose.Schema({
    CardID: {
        type: String,
        required: true,
      },
    BoardID: {
    type: String,
    required: true,
  },
   CardName: {
    type: String,
  },
   CardDescription: {
    type: String,
  },
    UserID: {
    type: String,
    required: true,
  },
    Status: {
    type: String,
  },
    Deadline: {
    type: String,
  },
});

module.exports = mongoose.model('Card', cardSchema);
