package com.loser.sbredis;

import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.test.context.junit4.SpringJUnit4ClassRunner;

import java.util.Date;

@RunWith(SpringJUnit4ClassRunner.class)
public class RedisTest {

        @Autowired
        private StringRedisTemplate stringRedisTemplate;

        @Test
        public void test() throws Exception {

            String value = "aaa";

            Date date = new Date();

            stringRedisTemplate.opsForValue().set(date.toString(), value);

            String str = stringRedisTemplate.opsForValue().get(date);

            System.out.println(date);

            System.out.println(str);

    }
}
