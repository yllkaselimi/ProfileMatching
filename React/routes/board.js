const express = require('express');
const router = express.Router();

// board model
const board = require('../models/board');

// @route   GET /api/board/
// @desc    Get all boards
// @access  Public
router.get('/', async (req, res) => {
  try {
    const board = await board.find({});
    res.send({ board })
  } catch(err) {
    res.status(400).send({ error: err });
  }
});

// @route   GET /api/board/:id
// @desc    Get a specific board
// @access  Public
router.get('/:id', async (req, res) => {
  try {
    const board = await board.findById(req.params.id);
    res.send({ board });
  } catch (err) {
    res.status(404).send({ message: 'board not found!' });
  }
});

// @route   POST /api/board/
// @desc    Create a board
// @access  Public
router.post('/', async (req, res) => {
  try {
    const newboard = await board.create({ name: req.body.name, email: req.body.email, enrollnumber: req.body.enrollnumber });
     res.send({ newboard });
  } catch(err) {
    res.status(400).send({ error: err });
  }

});

// @route   PUT /api/board/:id
// @desc    Update a board
// @access  Public
router.put('/:id', async (req, res) => {
  try {
    const updatedboard = await board.findByIdAndUpdate(req.params.id, req.body);
     res.send({ message: 'The board was updated' });
  } catch(err) {
    res.status(400).send({ error: err });
  }
});

// @route   DELETE /api/board/:id
// @desc    Delete a board
// @access  Public
router.delete('/:id', async (req, res) => {
  try {
    const removeboard = await board.findByIdAndRemove(req.params.id);
     res.send({ message: 'The board was removed' });
  } catch(err) {
    res.status(400).send({ error: err });
  }
});


module.exports = router;