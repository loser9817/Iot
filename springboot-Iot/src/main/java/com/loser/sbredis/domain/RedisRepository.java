package com.loser.sbredis.domain;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.StringRedisTemplate;
import org.springframework.stereotype.Component;

import java.util.*;
import java.util.concurrent.TimeUnit;

@Component
public class RedisRepository {

    @Autowired
    private StringRedisTemplate stringRedisTemplate;

    public Sensor save5Sec(String value) {

        String key = String.valueOf(new Date().getTime());

        stringRedisTemplate.opsForValue().set(key, value, 5, TimeUnit.SECONDS);

        return new Sensor(key, stringRedisTemplate.opsForValue().get(key));

    }

    public Sensor save(String value) {
        String key = String.valueOf(new Date().getTime());
        stringRedisTemplate.opsForValue().set(key, value);
        return new Sensor(key, stringRedisTemplate.opsForValue().get(key));
    }

    public List<Sensor> list() {

        List<Sensor> list = new ArrayList<>();

        Set<String> keys = stringRedisTemplate.keys("*");


        for (String key : keys) {

            String value = stringRedisTemplate.opsForValue().get(key);

            list.add(new Sensor(key, value));
        }

        return list;
    }

    public Sensor getNew (){
        List<Sensor> list = list();
        long maxKey = 0;
        int maxIndex = 0;
        for (int i = 0; i < list.size()-1; i++){
            Sensor sensor = list.get(i);
            long key = Long.parseLong(sensor.getKey());

            if (key > maxKey){
                maxKey = key;
                maxIndex = i;
            }
        }

        return list.get(maxIndex);
    }

}
