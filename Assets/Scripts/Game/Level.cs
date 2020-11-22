﻿using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_SIZE = 50f;
    private const float PIPE_MOVE_SPEED = 25f;
    private const float CAMERA_LEFT_EDGE = -100f;
    private const float CAMERA_RIGHT_EDGE = 100f;

    private List<Pipe> pipes;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gap;

    private void Awake()
    {
        pipes = new List<Pipe>();
        gap = 30f;
        pipeSpawnTimerMax = 1.5f;
    }

    private void Start()
    {

    }

    private void Update()
    {
        HandlePipeMoviment();
        HandlePipeSpawning();
    }

    private void HandlePipeMoviment()
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            var pipe = pipes[i];

            pipe.Move(PIPE_MOVE_SPEED);

            if (pipe.XPosition < CAMERA_LEFT_EDGE)
            {
                pipe.DestroySelf();
                pipes.Remove(pipe);
                i--;
            }
        }
    }

    private void HandlePipeSpawning()
    {
        float getHeight()
        {
            float heightEdgeLimit = 30f;
            float minHeight = gap * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_SIZE * 2f;
            float maxHeight = totalHeight - gap * .5f - heightEdgeLimit;

            return Random.Range(minHeight, maxHeight);
        }

        pipeSpawnTimer -= Time.deltaTime;

        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;

            float height = getHeight();
            CreateGapPipes(height, gap, CAMERA_RIGHT_EDGE);
        }
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition);
        CreatePipe(CAMERA_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createOnBottom = true)
        => pipes.Add(new Pipe(height, xPosition, createOnBottom));

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Impossible:
                gap = 15f;
                pipeSpawnTimerMax = 1f;
                break;
            case Difficulty.Hard:
                gap = 20f;
                pipeSpawnTimerMax = 1.15f;
                break;
            case Difficulty.Medium:
                gap = 25f;
                pipeSpawnTimerMax = 1.35f;
                break;
            case Difficulty.Easy:
            default:
                // Definidos no Awake
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }

    private class Pipe
    {
        private const float PIPE_BODY_WIDTH = 7.8f;
        private const float PIPE_HEAD_HEIGHT = 3.75f;

        private Transform _head;
        private Transform _body;

        public Pipe(float height, float xPosition, bool createOnBottom)
        {
            CreateHead(height, xPosition, createOnBottom);
            CreateBody(height, xPosition, createOnBottom);
        }

        private void CreateHead(float height, float xPosition, bool createOnBottom)
        {
            _head = Instantiate(GameAssets.GetInstance().PipeHead);
            float pipeHeadYPosition;
            if (createOnBottom)
                pipeHeadYPosition = -CAMERA_SIZE + height - PIPE_HEAD_HEIGHT * .48f;
            else
                pipeHeadYPosition = CAMERA_SIZE - height + PIPE_HEAD_HEIGHT * .48f;
            _head.position = new Vector3(xPosition, pipeHeadYPosition);
        }

        private void CreateBody(float height, float xPosition, bool createOnBottom)
        {
            _body = Instantiate(GameAssets.GetInstance().PipeBody);
            float pipeBodyYPosition;
            if (createOnBottom)
                pipeBodyYPosition = -CAMERA_SIZE;
            else
            {
                pipeBodyYPosition = CAMERA_SIZE;
                _body.localScale = new Vector3(1, -1, 1);
            }
            _body.position = new Vector3(xPosition, pipeBodyYPosition);

            SpriteRenderer pipeBodySpriteRenderer = _body.GetComponent<SpriteRenderer>();
            pipeBodySpriteRenderer.size = new Vector2(PIPE_BODY_WIDTH, height);

            BoxCollider2D pipeBodyCollider = _body.GetComponent<BoxCollider2D>();
            pipeBodyCollider.size = new Vector2(PIPE_BODY_WIDTH, height);
            pipeBodyCollider.offset = new Vector2(0f, height * .5f);
        }

        public void Move(float moveSpeed)
        {
            _head.position += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
            _body.position += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        }

        public float XPosition => _head.position.x;

        public void DestroySelf()
        {
            Destroy(_head.gameObject);
            Destroy(_body.gameObject);
        }
    }
}