package com.loser.sbredis.controller;

import com.loser.sbredis.domain.RedisRepository;
import com.loser.sbredis.domain.Sensor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.List;

@Controller
public class RedisController {

    @Autowired
    private RedisRepository redisRepository;


    //有过期的方法
    @GetMapping("/v/{value}")
    @ResponseBody
    public Sensor set(@PathVariable("value") String value){

        return redisRepository.save5Sec(value);

    }
    //持久化的方法
    @GetMapping("/r/{value}")
    @ResponseBody
    public Sensor setForever(@PathVariable("value") String value){

        return redisRepository.save(value);

    }

    @GetMapping("/")
    @ResponseBody
    public List<Sensor> get(){
        return redisRepository.list();
    }

    @GetMapping("/page")
    public String page(){
        return "list.html";
    }

    @GetMapping("/new")
    @ResponseBody
    public Sensor newSensor (){

        return redisRepository.getNew();
    }

}
